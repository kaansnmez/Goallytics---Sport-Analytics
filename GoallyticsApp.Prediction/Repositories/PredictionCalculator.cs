using AutoMapper;
using GoallyticsApp.Application.Dtos.Forecast;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoallyticsApp.Prediction.Repositories
{
    public class PredictionCalculator : IPredictionCalculator
    {
        private readonly IUow _uow;
        private readonly ILogger<PredictionCalculator> _logger;
        private readonly IMapper _mapper;

        //Constants
        private const double XG_WEIGHT = 0.4;
        private const double BASE_WEIGHT = 0.6;
        private const double H2H_BASE_WEIGHT = 0.3;
        private const double H2H_BONUS_PER_MATCH = 0.025;
        private const double MAX_H2H_WEIGHT = 0.4;
        private const int RECENT_FORM_WINDOW = 10;

        public PredictionCalculator(ILogger<PredictionCalculator> logger, IUow uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<TeamHistoryForcastDto>> GetTeamHistoryAsync (int teamId,DateTime beforeDate , CancellationToken ct = default)
        {
            var cutoffDate = beforeDate.AddYears(-2);
            var history = await _uow.GetRepository<Fixtures>().QueryToListAsync(f => (f.HomeTeamId == teamId || f.AwayTeamId == teamId) && (f.DateUtc < beforeDate && f.DateUtc >= cutoffDate) && f.StatusShort=="FT", f => f.DateUtc, 0,ct);
            var dto = new List<TeamHistoryForcastDto>();
            foreach (var historyItem in history)
            {
                if (historyItem != null)
                {
                    dto.Add(new TeamHistoryForcastDto
                    {
                        DateUtc = historyItem.DateUtc,
                        IsHome = historyItem.HomeTeamId==teamId,
                        GoalsFor = historyItem.HomeTeamId== teamId ? (historyItem.HomeScore??-1) : (historyItem.AwayScore??-1),
                        GoalsAgainst = historyItem.HomeTeamId == teamId ? (historyItem.AwayScore ?? -1) : (historyItem.HomeScore ?? -1),
                        XG = null,
                        XGA = null,
                    });
                }
            }
            return dto;

        }
        public async Task<List<H2HMatchForcastDto>> GetH2HMatchesAsync(int team1Id, int team2Id,DateTime beforeDate, CancellationToken ct = default)
        {
            var cutoffDate = beforeDate.AddYears(-3);
            var history = await _uow.GetRepository<Fixtures>().QueryToListAsync(f =>
            (f.HomeTeamId == team1Id && f.AwayTeamId == team2Id) && (f.HomeTeamId == team2Id && f.AwayTeamId == team1Id)
            && f.DateUtc >= cutoffDate && f.DateUtc < beforeDate
            && f.StatusShort == "FT", f => f.DateUtc, 0, ct);
            var dto = new List<H2HMatchForcastDto>();
            foreach (var historyItem in history)
            {
                if (historyItem != null)
                {
                    dto.Add(new H2HMatchForcastDto
                    {
                        DateUtc = historyItem.DateUtc,
                        HomeTeamId = historyItem.HomeTeamId,
                        AwayTeamId = historyItem.AwayTeamId,
                        HomeScore = historyItem.HomeScore?? -1,
                        AwayScore = historyItem.AwayScore ?? -1
                    });
                }
            }
            return dto;
        }
        public ExpectedGoalsForcastDto ComputeExpectedGoals(List<TeamHistoryForcastDto> homeHist,List<TeamHistoryForcastDto> awayHist,List<H2HMatchForcastDto> h2hHist)
        {
            var homeGpg = homeHist.Average(m => m.GoalsFor);
            var homeGag = homeHist.Average(m=>m.GoalsAgainst);
            var awayGpg = awayHist.Average(m => m.GoalsFor);
            var awayGag = awayHist.Average(m => m.GoalsAgainst);

            var homeExpected = (homeGpg + awayGag) / 2.0;
            var awayExpected = (awayGpg + homeGag) / 2.0;
            var totalExpected = homeExpected+ awayExpected;

            if (h2hHist.Count >= 2)
            {
                var h2hGpg = h2hHist.Average(m => m.HomeScore + m.AwayScore);
                var h2hWeight = Math.Min(MAX_H2H_WEIGHT,H2H_BASE_WEIGHT+Math.Min(0.1,H2H_BONUS_PER_MATCH+h2hHist.Count));
                totalExpected = (totalExpected * (1 - h2hWeight)) + (h2hGpg * h2hWeight);
                var ratio = (homeExpected + awayExpected) > 0
                    ? totalExpected / (awayExpected + homeExpected)
                    : 1.0;
                homeExpected*=ratio;
                awayExpected*=ratio;
            }
            return new ExpectedGoalsForcastDto
            {
                TotalExpected = totalExpected,
                HomeExpected = homeExpected,
                AwayExpected = awayExpected,
                BaseTotal = totalExpected,
                XgTotal = totalExpected,
                H2HCount = h2hHist.Count,
            };

        }
        #region Math Helpers
        public static double PoissonPmf(int k, double lambda)
        {
            if (lambda <= 0)
                return k == 0 ? 1.0 : 0.0;
            return Math.Exp(-lambda) * Math.Pow(lambda, k) / Factorial(k);
        }
        public static double Factorial(int n)
        {
            if ((n <= 1))
                return 1.0;
            double result = 1.0;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }
        #endregion


        #region Prediction Methods
        public PredictionResultForcastDto PredictOverUnder(ExpectedGoalsForcastDto expected,double line ,int fixtureId,int homeTeamId,int awayTeamId,string market)
        {
            var prediction = expected.TotalExpected > line ? "Over" : "Under";
            var confidence = Math.Min(95,50+Math.Abs(expected.TotalExpected-line)*20);
            var anaylsis = new
            {
                line,
                total_expected = Math.Round(expected.TotalExpected,2),
                base_total  = Math.Round(expected.BaseTotal,2),
                xg_total = Math.Round(expected.XgTotal,2),
                h2h_count = expected.H2HCount
            };
            return new PredictionResultForcastDto
            {
                FixtureId = fixtureId,
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                Market = market,
                Prediction = prediction,
                Confidence = Math.Round(confidence, 1),
                ExpectedGoals = Math.Round(expected.TotalExpected, 2),
                Analysis = JsonSerializer.Serialize(anaylsis)
            };
        }
        
        public PredictionResultForcastDto PredictBTTS(ExpectedGoalsForcastDto expected,List<TeamHistoryForcastDto> homeHist,List<TeamHistoryForcastDto> awayHist,List<H2HMatchForcastDto> h2hHist,int fixtureId,int homeTeamId,int awayTeamId)
        {
            //poisson base
            var pHomeScoringBase = 1 - PoissonPmf(0, expected.HomeExpected);
            var pAwayScoringBase = 1 - PoissonPmf(0,expected.AwayExpected);
            var pBttsBase = pHomeScoringBase * pAwayScoringBase;

            // Recent Form
            var homeRecent = homeHist.Where(m=>m.IsHome).TakeLast(RECENT_FORM_WINDOW).ToList();
            var homeScoringRate = homeRecent.Count >= 3
                ? homeRecent.Count(m => m.GoalsFor > 0) / (double)homeRecent.Count
                : pHomeScoringBase;

            var awayRecent = awayHist.Where(m => m.IsHome).TakeLast(RECENT_FORM_WINDOW).ToList();
            var awayScoringRate = awayRecent.Count >= 3
                ? awayRecent.Count(m => m.GoalsFor > 0) / (double)awayRecent.Count
                : pAwayScoringBase;

            var pBttsForm = homeScoringRate * awayScoringRate;


            // h2h btts

            double pBtts;
            if (h2hHist.Count >= 3)
            {
                var h2hBttsRate = h2hHist.Count(m => m.HomeScore > 0 && m.AwayScore > 0) / (double)h2hHist.Count;
                pBtts = (0.4 * pBttsBase) + (0.45 * pBttsForm) + (0.15 * h2hBttsRate);
            }
            else
            {
                pBtts = (0.45 * pBttsBase) + (0.55 * pBttsForm);
            }
            var prediction = pBtts > 0.5 ? "Yes" : "No";
            var confidence = Math.Min(95, 50 + Math.Abs(pBtts - 0.5) * 90);
            var analysis = new
            {
                btts_prob = Math.Round(pBtts, 3),
                base_prob = Math.Round(pBttsBase, 3),
                form_prob = Math.Round(pBttsForm, 3),
                home_scoring_rate = Math.Round(homeScoringRate, 3),
                away_scoring_rate = Math.Round(awayScoringRate, 3),
                h2h_count = h2hHist.Count
            };
            return new PredictionResultForcastDto
            {
                FixtureId = fixtureId,
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                Market = "BTTS",
                Prediction = prediction,
                Confidence = Math.Round(confidence, 1),
                ExpectedGoals = Math.Round(expected.TotalExpected, 2),
                Analysis = JsonSerializer.Serialize(analysis)
            };
        }
        public PredictionResultForcastDto? Predict1X2 (ExpectedGoalsForcastDto expected,int fixtureId,int homeTeamId,int awayTeamId)
        {
            var homeLambda = expected.HomeExpected;
            var awayLambda = expected.AwayExpected;

            double pHome = 0.0 , pDraw=0.0 , pAway = 0.0 ;
            for (int h = 0; h < 5; h++)
            {
                for (int w = 0; w < 5; w++)
                {
                    var prop = PoissonPmf(h, homeLambda) * PoissonPmf(w, awayLambda);
                    if(h>w) pHome += prop;
                    else if (h==w) pDraw += prop;
                    else pAway += prop;
                }

            }
            var total = pHome + pDraw + pAway;
            pHome /= total;
            pDraw /= total;
            pAway /= total;

            var props = new[] { ("Home", pHome), ("Draw", pDraw), ("Away", pAway) };
            var (prediction, maxProp) = props.OrderByDescending(p => p.Item2).First(); ;
            var confidence = Math.Min(95, maxProp * 100);

            var analysis = new
            {
                home_prob = Math.Round(pHome, 3),
                draw_prob = Math.Round(pDraw, 3),
                away_prob = Math.Round(pAway, 3),
                home_expected = Math.Round(homeLambda, 2),
                away_expected = Math.Round(awayLambda, 2)
            };
            return new PredictionResultForcastDto
            {
                FixtureId = fixtureId,
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                Market = "1X2",
                Prediction = prediction,
                Confidence = Math.Round(confidence, 1),
                ExpectedGoals = Math.Round(expected.TotalExpected, 2),
                Analysis = JsonSerializer.Serialize(analysis)
            };
        }
        #endregion
        public async Task<AllMarketPredictionsForcastDto?> CalculateAllMarketsAsync(int fixtureId, int homeTeamId, int awayTeamId, DateTime fixtureDate, CancellationToken ct = default)
        {
            var homeHistory = await GetTeamHistoryAsync(homeTeamId, fixtureDate, ct);
            var awayHistory = await GetTeamHistoryAsync(awayTeamId, fixtureDate, ct);
            if ((!homeHistory.Any() || !awayHistory.Any()) && (homeHistory.Count < 5 || awayHistory.Count < 5))
            {
                _logger.LogWarning("Insfuficent history for teams {Home}/{Away}",homeTeamId, awayTeamId);
                return null;
            }
            var h2hMatches = await GetH2HMatchesAsync(homeTeamId,awayTeamId, fixtureDate, ct);
            var expectedGoals = ComputeExpectedGoals(homeHistory,awayHistory, h2hMatches);

            var ou15 = PredictOverUnder(expectedGoals,1.5,fixtureId, homeTeamId,awayTeamId,"OU15");
            var ou25 = PredictOverUnder(expectedGoals, 2.5, fixtureId, homeTeamId, awayTeamId, "OU25");
            var ou35 = PredictOverUnder(expectedGoals, 3.5, fixtureId, homeTeamId, awayTeamId, "OU35");

            var btts = PredictBTTS(expectedGoals,homeHistory,awayHistory,h2hMatches,fixtureId,homeTeamId,awayTeamId);
            var result1x2 = Predict1X2(expectedGoals, fixtureId, homeTeamId, awayTeamId);

            return new AllMarketPredictionsForcastDto
            {
                FixtureId = fixtureId,
                DateUtc = fixtureDate,
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                OU15 = ou15,
                OU25 = ou25,
                OU35 = ou35,
                BTTS = btts,
                Result1x2 = result1x2,
            };
        }
    }
}

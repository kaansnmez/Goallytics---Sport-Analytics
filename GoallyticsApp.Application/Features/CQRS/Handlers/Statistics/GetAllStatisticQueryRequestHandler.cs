using GoallyticsApp.Application.Dtos.Statistics;
using GoallyticsApp.Application.Features.CQRS.Queries.Statistics;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Statistics
{
    public class GetAllStatisticQueryRequestHandler : IRequestHandler<GetAllStatisticQueryRequest, StatisticViewDto>
    {
        private readonly IUow _uow;

        public GetAllStatisticQueryRequestHandler(IUow uow)
        {
            _uow = uow;
        }

        public async Task<StatisticViewDto> Handle(GetAllStatisticQueryRequest request, CancellationToken cancellationToken)
        {
            var dto =  new StatisticViewDto();
            dto.General = GeneralStats(cancellationToken).Result;
            dto.CurrentPeriod=request.Period;
            if(dto.CurrentPeriod=="current")
            {
                dto.WeeklyPrediction= await WeeklyPrediction(cancellationToken);
            }
            else
            {
                dto.WeeklyPrediction = await WeeklyPrediction(cancellationToken, true);
            }
            dto.Charts = await ChartsData(cancellationToken);
            dto.Markets = await MarketsData(cancellationToken);
            dto.Insights = await InsightsData(cancellationToken);

            return dto;
        }
        public async Task<GeneralStatsDto> GeneralStats(CancellationToken ct)
        {
            var dto = new GeneralStatsDto();
            var result = await _uow.GetRepository<Prediction>().QueryToListAsync(null, null, 0, ct);
            var totalMatch = _uow.GetRepository<Fixtures>().GetAllAsync().Result.Count();
            var totalPredict = result.Count;
            var correctPredict = result.Where(f => f.Result == true).Count();
            var accuracyRate = Math.Round((double)(correctPredict / (double)totalPredict), 4)*100.00;

            dto.TotalPredictions = totalPredict;
            dto.CorrectPredictions = correctPredict;
            dto.AccuracyRate = accuracyRate;
            dto.TotalMatches = totalMatch;
            
            return dto;
        }
        public async Task<List<WeeklyPredictionRowDto>> WeeklyPrediction(CancellationToken ct, bool prevWeek = false)
        {
            var list = new List<WeeklyPredictionRowDto>();
            CultureInfo myCI = new CultureInfo("tr-TR");
            Calendar myCal = myCI.Calendar;

            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            DateTime now = DateTime.Now;
            int nowWeek = 7;
            var result = new List<Fixtures>();
            if (prevWeek)
            {
                nowWeek = -7;
                result = await _uow.GetRepository<Fixtures>().QueryToListAsync(f => (f.DateUtc > now.AddDays(nowWeek) && f.DateUtc < now), null, 0, ct, f => f.Predictions, f => f.HomeTeam, f => f.AwayTeam, f => f.League);
            }
            else
                result = await _uow.GetRepository<Fixtures>().QueryToListAsync(f => (f.DateUtc < now.AddDays(nowWeek) && f.DateUtc > now), null, 0, ct, f => f.Predictions, f => f.HomeTeam, f => f.AwayTeam, f => f.League);
            //var result = await _uow.GetRepository<Fixtures>().QueryToListAsync(f=> myCal.GetWeekOfYear(f.DateUtc, myCWR, myFirstDOW)==nowWeek, null, 0, ct,f=>f.Predictions,f=>f.HomeTeam,f=>f.AwayTeam,f=>f.League);
            foreach (var item in result)
            {
                if(item.Predictions!=null && item.Predictions.Count>0)
                {
                    var pred = item.Predictions.OrderByDescending(f => f.Confidence).First();
                    list.Add(new WeeklyPredictionRowDto()
                    {
                        Date = item.DateUtc,
                        League = item.League.Name,
                        HomeTeam = item.HomeTeam.Name,
                        AwayTeam = item.AwayTeam.Name,
                        Market = pred.Market,
                        Prediction = pred.Forecast,
                        Confidence = pred.Confidence,
                        Result = pred.Result == null ? "Pending" : (pred.Result == true ? "Correct" : "Incorrect"),
                        IsCorrect = pred.Result,
                        Status = item.StatusShort
                    });
                }
            }
            
            
            return list;
        }
        public async Task<ChartsDataDto> ChartsData (CancellationToken ct)
        {
            var dto = new ChartsDataDto();
            //Market Accuracy
            var preds = await _uow.GetRepository<Prediction>().QueryToListAsync(null,null,0,ct);
            var accuracyRateMarkets= preds.GroupBy(f => f.Market).Select(g => new { 
                Market = g.Key, 
                Total = g.Count(), 
                Correct = g.Where(f => f.Result == true).Count(),
                Acc= Math.Round((double)g.Sum(f => g.Where(p => p.Result == true).Count()) / (double)g.Sum(f => g.Count()), 4) * 100
            }).ToList();
            foreach (var item in accuracyRateMarkets)
            {
                dto.MarketAccuracy.Labels.Add(item.Market);
                dto.MarketAccuracy.Accuracies.Add(item.Acc);
            }
            //League Accuracy
            var fixtures = await _uow.GetRepository<Fixtures>().QueryToListAsync(null, null, 0, ct, f => f.League, f => f.Predictions);
            var leagueAccuracies = fixtures.GroupBy(f => new { f.League.LeagueApiId, f.League.Name }).Select(g => new { 
                League = g.Key, 
                Total = g.Sum(f => f.Predictions.Count), 
                Correct = g.Sum(f => f.Predictions.Where(p => p.Result == true).Count()),
                Acc = Math.Round((double)g.Sum(f => f.Predictions.Where(p => p.Result == true).Count()) / (double)g.Sum(f => f.Predictions.Count()), 4)*100
            }).ToList();
            foreach (var item in leagueAccuracies)
            {
                dto.LeagueAccuracy.Leagues.Add(item.League.Name);
                dto.LeagueAccuracy.Accuracies.Add(item.Acc);
            }
            //Monthly Performance
            var monthlyPerformances = fixtures.Where(f=>f.Year==2025).GroupBy(f => f.DateUtc.Month ).Select(g => new {
                Month = g.Key, 
                Total = g.Sum(f => f.Predictions.Count), 
                Correct = g.Sum(f => f.Predictions.Where(p => p.Result == true).Count()),
                Incorrect = g.Sum(f => f.Predictions.Where(p => p.Result != true).Count()),
                Accuracy = Math.Round((double)g.Sum(f => f.Predictions.Where(p => p.Result == true).Count()) / (double)g.Sum(f=>f.Predictions.Count),4)*100 }).OrderBy(f => f.Month).ThenBy(f => f.Month).ToList();
            foreach (var item in monthlyPerformances)
            {
                dto.MonthlyPerformance.Months.Add(item.Month.ToString());
                dto.MonthlyPerformance.Correct.Add(item.Correct);
                dto.MonthlyPerformance.Incorrect.Add(item.Total - item.Correct);
                dto.MonthlyPerformance.Accuracy.Add(double.IsNaN(Math.Round(Math.Abs((double)item.Accuracy),2)) ? 0 : Math.Round(Math.Abs((double)item.Accuracy), 2));
            }
            return dto;
        }
        public async Task<MarketsDataDto> MarketsData (CancellationToken ct)
        {
            var dto = new MarketsDataDto();
            var preds = await _uow.GetRepository<Prediction>().QueryToListAsync(null, null, 0, ct);
            var marketStats = preds.GroupBy(f => f.Market).ToDictionary(
                g=>g.Key,
                g =>new MarketCardStatsDto
            {
                
                Total = g.Count(),
                Correct = g.Where(f => f.Result == true).Count(),
                Accuracy = Math.Round((double)g.Where(f => f.Result == true).Count() / g.Count(), 4)*100
            });
            //Market Distribution
            foreach (var item in marketStats)
            {
                switch (item.Key)
                {
                    case "1X2":
                        dto.Market1x2 = item.Value;
                        continue;
                    case "OU15":
                        dto.OU15 = item.Value;
                        continue;
                    case "OU25":
                        dto.OU25 = item.Value;
                        continue;
                    case "OU35":
                        dto.OU35 = item.Value;
                        continue;
                    case "BTTS":
                        dto.BTTS = item.Value;
                        continue;
                    default:
                        break;
                }   
                
            }

            return dto;
        }
        public async Task<InsightsDataDto> InsightsData (CancellationToken ct)
        {
            var dto = new InsightsDataDto();
            var preds = await _uow.GetRepository<Prediction>().QueryToListAsync(null, null, 0, ct);
            var leagues = await _uow.GetRepository<Leagues>().QueryToListAsync(null, null, 0, ct);

            var bestMarket = preds.GroupBy(f => f.Market).Select(g => new { Market = g.Key, Total = g.Count(), Correct = g.Where(f => f.Result == true).Count(), Accuracy = Math.Round((double)g.Where(f => f.Result == true).Count() / g.Count(), 2) }).OrderByDescending(f => f.Accuracy).FirstOrDefault();
            var bestLeague = leagues.Select(l => new
            {
                League = l.Name,
                Total = preds.Where(f =>l.LeagueApiId == f.LeaugeId).Count(),
                Correct = preds.Where(f =>l.LeagueApiId == f.LeaugeId && f.Result == true).Count()
            }).Select(g => new
            {
                g.League,
                g.Total,
                g.Correct,
                Accuracy = g.Total > 0 ? Math.Round((double)g.Correct / g.Total, 4)*100 : 0
            }).OrderByDescending(f => f.Accuracy).FirstOrDefault();
            var AccuracyMeanScore = preds.Count > 0 ? Math.Round((double)preds.Where(f => f.Result == true).Count() / preds.Count, 4) * 100 : 0;
            dto.BestMarket = bestMarket == null ? "N/A" : bestMarket.Market;
            dto.BestMarketAccuracy = bestMarket == null ? 0 : (double)bestMarket.Accuracy ;
            dto.BestLeague = bestLeague == null ? "N/A" : bestLeague.League;
            dto.BestLeagueAccuracy = bestLeague == null ? 0 : (double)bestLeague.Accuracy ;
            dto.AvgConfidence = (double)AccuracyMeanScore;

            return dto;
        }

    }
}

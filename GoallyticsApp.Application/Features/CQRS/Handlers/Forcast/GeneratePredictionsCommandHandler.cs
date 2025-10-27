using AutoMapper.Features;
using GoallyticsApp.Application.Dtos.Forecast;
using GoallyticsApp.Application.Features.CQRS.Commands.Forcast;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Forcast
{
    public class GeneratePredictionsCommandHandler : IRequestHandler<GeneratePredictionsCommand, GeneratePredictionsResultDto>
    {
        private readonly IUow _uow;
        private readonly IPredictionCalculator _predictionCalculator;
        private readonly ILogger<GeneratePredictionsCommandHandler> _logger;

        public GeneratePredictionsCommandHandler(ILogger<GeneratePredictionsCommandHandler> logger, IUow uow, IPredictionCalculator predictionCalculator)
        {
            _logger = logger;
            _uow = uow;
            _predictionCalculator = predictionCalculator;
        }

        public async Task<GeneratePredictionsResultDto> Handle(GeneratePredictionsCommand request, CancellationToken ct)
        {
            var totalFixtures =0;
            var generated = 0;
            var skipped = 0;
            var errors = new List<string>();
            var createdAt =await _uow.GetRepository<Prediction>().QueryToListAsync(null,f=>f.CreatedAt,1,ct);
            if (!(createdAt.Count > 0))
                createdAt.Add(new() { DateUtc = DateTime.UtcNow.Date.AddDays(-1) });
            var startDate = request.StartDate ?? DateTime.UtcNow.Date;
            if (!(createdAt[0].CreatedAt > startDate))
            {



                var endDate = request.EndDate ?? startDate.AddDays(3);

                _logger.LogInformation("Generating predictions for {Start} to {end} , League: {leauge}", startDate, endDate, request.LeagueId);
                var fixtures = await _uow.GetRepository<Fixtures>().QueryToListAsync(f => (f.DateUtc < endDate && f.DateUtc >= startDate) && f.Predictions.Count==0, null, 0, ct, f => f.League,f=>f.Predictions);
                 totalFixtures = fixtures.Count;
                 
                foreach (var fixture in fixtures)
                {
                    try
                    {
                        var predictions = await _predictionCalculator.CalculateAllMarketsAsync(
                            fixture.FixturesId,
                            fixture.HomeTeamId,
                            fixture.AwayTeamId,
                            fixture.DateUtc,
                            ct);
                        if (predictions == null)
                        {
                            _logger.LogDebug("Prediction is null - FixtureId: {FixtureId}, LeaugeId: {Leauge}",
                            fixture.FixturesId,fixture.League.LeagueApiId);
                            skipped++;
                            continue;
                        }
                        var savedCount = await SavePredictionAsync(predictions, fixture, request.MinConfidenceThreshold, ct);
                        generated += savedCount;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to generate prediction for fixture {Id}", fixture.FixturesId);
                        errors.Add($"Fixture {fixture.Id}:{ex.Message}");
                    }
                }
                
                _logger.LogInformation("Prediction generation complete. Total: {Total}, Generated: {Generated}, Skipped: {Skipped}",
                totalFixtures, generated, skipped);
                var activeCount = await StatusActive(ct);
                _logger.LogInformation("Active predictions to change Inactive status. Total: {activeCount}",
               activeCount);
                var resultCount = await CheckMatchResult(ct);
                _logger.LogInformation("Changed Prediction Result count. Total: {resultCount}",
                activeCount);
            }
            else
            {
                var resultCount = await CheckMatchResult(ct);
                _logger.LogInformation("Changed Prediction Result count. Total: {resultCount}",
                resultCount);

                var activeCount = await StatusActive(ct);
                _logger.LogInformation("Active predictions to change Inactive status. Total: {activeCount}",
                activeCount);
                _logger.LogInformation("Prediction generation canceled.Last prediction is {date} .",createdAt);
                await _uow.SaveChangesAsync();
                return new GeneratePredictionsResultDto
                {
                    TotalFixtures = 0,
                    PredictionsGenerated = 0,
                    Skipped = 0,
                    Errors = new()
                };
            }

            await _uow.SaveChangesAsync();

            return new GeneratePredictionsResultDto
            {
                TotalFixtures = totalFixtures,
                PredictionsGenerated = generated,
                Skipped = skipped,
                Errors = errors
            };

        }
        private async Task<int> SavePredictionAsync(AllMarketPredictionsForcastDto predictions, Fixtures fixture, int minThreshold, CancellationToken ct = default)
        {
            var count = 0;
            var markets = new[] { predictions.OU15, predictions.OU25, predictions.OU35, predictions.BTTS };
            if (predictions.Result1x2 != null)
            {
                markets = markets.Append(predictions.Result1x2).ToArray();
            }
            foreach (var pred in markets)
            {
                if (pred.Confidence < minThreshold)
                {
                    _logger.LogInformation("Düşük confidence atlandı - FixtureId: {FixtureId}, Leagues: {Leagues}, Market: {Market}, Confidence: {Confidence:F1} < {Threshold}",
                                            fixture.FixturesId, pred.Market,fixture.League.LeagueApiId, pred.Confidence, minThreshold);
                    continue;
                }
                    
                await _uow.GetRepository<Prediction>().CreateAsync(new Prediction
                {
                    FixtureId = fixture.FixturesId,
                    LeaugeId = fixture.League.LeagueApiId,
                    DateUtc = fixture.DateUtc,
                    HomeTeamId = fixture.HomeTeamId,
                    AwayTeamId = fixture.AwayTeamId,
                    Market = pred.Market,
                    Forecast = pred.Prediction,
                    Confidence = pred.Confidence,
                    ExpectedGoals = pred.ExpectedGoals,
                    HomeExpected = predictions.OU15.ExpectedGoals,
                    AwayExpected = 0,
                    AnalysisJson = pred.Analysis,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                });
                count++;
                _logger.LogDebug("Prediction kaydedildi - FixtureId: {FixtureId}, Market: {Market}, Confidence: {Confidence:F1}",
                fixture.FixturesId, pred.Market, pred.Confidence);
            }
            _logger.LogInformation("Prediction batch tamamlandı - FixtureId: {FixtureId}, Kaydedilen: {Count}",
                fixture.FixturesId, count);
            return count;
        }
        private async Task<int> StatusActive (CancellationToken ct)
        {
            var count = 0;
            var today = DateTime.UtcNow;
            var statusActive = await _uow.GetRepository<Prediction>().QueryToListAsync(f => f.IsActive == true);
            if (statusActive.Count != 0)
            {
                foreach (var pred in statusActive)
                {
                    if (pred.DateUtc < today)
                    {
                        count++;
                        pred.IsActive = false;
                    }

                }
                
            }
            return count;
        }
        private async Task<int> CheckMatchResult(CancellationToken ct)
        {
            var count = 0;
            var notActive = await _uow.GetRepository<Prediction>().QueryToListAsync(f => f.IsActive == false, null, 0, ct);
            //var fixtures = await _uow.GetRepository<Fixtures>().QueryToListAsync(f=>f.Predictions.Count!=0,null,0,ct,f=>f.Predictions);
            if (notActive.Count != 0)
            {
                foreach (var pred in notActive)
                {
                    var selectedFixtures = await _uow.GetRepository<Fixtures>().QueryToListAsync(f => f.FixturesId == pred.FixtureId, null, 0, ct, f => f.Predictions);
                    var selectedFixture = selectedFixtures.FirstOrDefault();
                    //var selectedFixture = fixtures.Where(f => f.FixturesId == pred.FixtureId).FirstOrDefault();
                    if (selectedFixture!=null)
                    {
                        switch (pred.Market)
                        {   
                            case "OU15":
                                var totalGoal = selectedFixture.HomeScore + selectedFixture.AwayScore;
                                if (totalGoal > 1 && pred.Forecast == "Over")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }  
                                if (totalGoal < 1 && pred.Forecast == "Under")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                    
                                pred.Result = false;
                                count++;
                                continue;
                            case "OU25":
                                totalGoal = selectedFixture.HomeScore + selectedFixture.AwayScore;
                                if (totalGoal > 2 && pred.Forecast == "Over")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                if (totalGoal < 2 && pred.Forecast == "Under")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                pred.Result = false;
                                count++;
                                continue;
                            case "OU35":
                                totalGoal = selectedFixture.HomeScore + selectedFixture.AwayScore;
                                if (totalGoal > 3 && pred.Forecast == "Over")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                if (totalGoal < 4 && pred.Forecast == "Under")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                pred.Result = false;
                                count++;
                                continue;
                            case "BTTS":
                                if (selectedFixture.HomeScore > 0 && selectedFixture.AwayScore >0 && pred.Forecast == "Yes")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                if (((selectedFixture.HomeScore > 0 && selectedFixture.AwayScore == 0) && 
                                    (selectedFixture.HomeScore == 0 && selectedFixture.AwayScore > 0) &&
                                    (selectedFixture.HomeScore + selectedFixture.AwayScore ==0))
                                    && pred.Forecast == "No")
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                pred.Result = false;
                                count++;
                                continue;
                            case "1X2":
                                if (pred.Forecast=="Home" && selectedFixture.HomeScore>selectedFixture.AwayScore)
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                if (pred.Forecast == "Away" && selectedFixture.HomeScore < selectedFixture.AwayScore)
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;
                                }
                                if (pred.Forecast == "Draw" && selectedFixture.HomeScore == selectedFixture.AwayScore)
                                {
                                    pred.Result = true;
                                    count++;
                                    continue;

                                }
                                count++;
                                pred.Result = false;
                                continue;
                            default:
                                break;
                        }
                    }
                }
            }
            return count;
        }
    }
}

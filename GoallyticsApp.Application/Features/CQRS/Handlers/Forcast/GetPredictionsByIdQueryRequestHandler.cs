using GoallyticsApp.Application.Dtos.Forecast;
using GoallyticsApp.Application.Features.CQRS.Queries.Predictions;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Forcast
{
    public class GetPredictionsByIdQueryRequestHandler : IRequestHandler<GetPredictionsByIdQueryRequest, GetPredictionRequestDto>
    {
        private readonly IUow _uow;
        private readonly ILogger<GeneratePredictionsCommandHandler> _logger;

        public GetPredictionsByIdQueryRequestHandler(ILogger<GeneratePredictionsCommandHandler> logger, IUow uow)
        {
            _logger = logger;
            _uow = uow;
        }

        public async Task<GetPredictionRequestDto> Handle(GetPredictionsByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var preds = await  _uow.GetRepository<Prediction>().QueryToListAsync(f=>f.FixtureId==request.FixtureId,null,0,cancellationToken);
            var dtos = new GetPredictionRequestDto();
            dtos.OU15 = new();
            dtos.OU25 = new();
            dtos.OU35 = new();
            dtos.BTTS = new();
            dtos.Result1x2 = new();
            dtos.FixtureId = request.FixtureId;
            foreach (var pred in preds) {
                dtos.DateUtc = pred.DateUtc;
                dtos.HomeTeamId = pred.HomeTeamId;
                dtos.AwayTeamId = pred.AwayTeamId;
                switch (pred.Market)
                {
                    case "OU15":
                        dtos.OU15.AwayTeamId = pred.AwayTeamId;
                        dtos.OU15.HomeTeamId = pred.HomeTeamId;
                        dtos.OU15.Prediction = pred.Forecast;
                        dtos.OU15.Confidence = pred.Confidence;
                        dtos.OU15.Market = pred.Market;
                        dtos.OU15.ExpectedGoals = pred.ExpectedGoals;
                        dtos.OU15.Analysis = pred.AnalysisJson;
                        continue;
                    case "OU25":
                        dtos.OU25.AwayTeamId = pred.AwayTeamId;
                        dtos.OU25.HomeTeamId = pred.HomeTeamId;
                        dtos.OU25.Prediction = pred.Forecast;
                        dtos.OU25.Confidence = pred.Confidence;
                        dtos.OU25.Market = pred.Market;
                        dtos.OU25.ExpectedGoals = pred.ExpectedGoals;
                        dtos.OU25.Analysis = pred.AnalysisJson;
                        continue;
                    case "OU35":
                        dtos.OU35.AwayTeamId = pred.AwayTeamId;
                        dtos.OU35.HomeTeamId = pred.HomeTeamId;
                        dtos.OU35.Prediction = pred.Forecast;
                        dtos.OU35.Confidence = pred.Confidence;
                        dtos.OU35.Market = pred.Market;
                        dtos.OU35.ExpectedGoals = pred.ExpectedGoals;
                        dtos.OU35.Analysis = pred.AnalysisJson;
                        continue;
                    case "BTTS":
                        dtos.BTTS.AwayTeamId = pred.AwayTeamId;
                        dtos.BTTS.HomeTeamId = pred.HomeTeamId;
                        dtos.BTTS.Prediction = pred.Forecast;
                        dtos.BTTS.Confidence = pred.Confidence;
                        dtos.BTTS.Market = pred.Market;
                        dtos.BTTS.ExpectedGoals = pred.ExpectedGoals;
                        dtos.BTTS.Analysis = pred.AnalysisJson;
                        continue;
                    case "1X2":
                        dtos.Result1x2.AwayTeamId = pred.AwayTeamId;
                        dtos.Result1x2.HomeTeamId = pred.HomeTeamId;
                        dtos.Result1x2.Prediction = pred.Forecast;
                        dtos.Result1x2.Confidence = pred.Confidence;
                        dtos.Result1x2.Market = pred.Market;
                        dtos.Result1x2.ExpectedGoals = pred.ExpectedGoals;
                        dtos.Result1x2.Analysis = pred.AnalysisJson;
                        continue;
                    default:
                        break;
                }
            }

            return dtos;

        }
    }
}

using GoallyticsApp.Application.Dtos.Forecast;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Queries.Predictions
{
    public class GetPredictionsByIdQueryRequest:IRequest<GetPredictionRequestDto>
    {
        public int FixtureId { get; set; }
    }
}

using GoallyticsApp.Application.Dtos.Statistics;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Queries.Statistics
{
    public class GetAllStatisticQueryRequest : IRequest<StatisticViewDto>
    {
        public string Period { get; set; } = "current";

    }
}

using GoallyticsApp.Application.Dtos.Forecast;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Commands.Forcast
{
    public class GeneratePredictionsCommand : IRequest<GeneratePredictionsResultDto>
    {
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public int? LeagueId { get; set; } = null;
        public int MinConfidenceThreshold { get; set; } = 61;


    }
}

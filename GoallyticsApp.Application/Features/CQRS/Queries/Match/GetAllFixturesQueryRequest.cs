using GoallyticsApp.Application.Dtos.Match;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Queries.Match
{
    public class GetAllFixturesQueryRequest : IRequest<List<GetAllFixturesDto>>
    {
        public DateTime DateUtc { get; set; } = DateTime.UtcNow;
        public int?  StatusShortId { get; set; }
        public int?  LeagueNameId { get; set; }

    }
}

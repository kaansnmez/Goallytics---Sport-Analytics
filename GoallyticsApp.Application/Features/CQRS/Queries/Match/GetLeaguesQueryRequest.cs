using GoallyticsApp.Application.Dtos.Match;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Queries.Match
{
    public class GetLeaguesQueryRequest : IRequest<List<GetLeaguesDto>>
    {

    }
}

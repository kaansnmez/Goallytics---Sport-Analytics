using AutoMapper;
using GoallyticsApp.Application.Dtos.Match;
using GoallyticsApp.Application.Features.CQRS.Queries.Match;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Match
{
    public class GetLeaguesQueryRequestHandler : IRequestHandler<GetLeaguesQueryRequest, List<GetLeaguesDto>>
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public GetLeaguesQueryRequestHandler(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<GetLeaguesDto>> Handle(GetLeaguesQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _uow.GetRepository<Leagues>().GetAllAsync();

            return _mapper.Map<List<GetLeaguesDto>>(result);
        }
    }
}

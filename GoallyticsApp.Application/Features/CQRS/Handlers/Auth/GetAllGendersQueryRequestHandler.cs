using AutoMapper;
using GoallyticsApp.Application.Dtos;
using GoallyticsApp.Application.Features.CQRS.Queries;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.AuthEntities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Auth
{
    public class GetAllGendersQueryRequestHandler : IRequestHandler<GetAllGendersQueryRequest, List<GenderDto>>
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public GetAllGendersQueryRequestHandler(IMapper mapper, IUow uow)
        {

            _mapper = mapper;
            _uow = uow;
        }

        public async Task<List<GenderDto>> Handle(GetAllGendersQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _uow.GetRepository<Gender>().GetAllAsync();
            if (result != null)
            {
                var genders = _mapper.Map<List<GenderDto>>(result);

                return genders;
            }
            else
                throw new Exception();
        }
    }
}

using AutoMapper;
using GoallyticsApp.Application.Dtos;
using GoallyticsApp.Application.Features.CQRS.Queries;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.AuthEntities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Auth
{
    public class CheckUserQueryRequestHandlers : IRequestHandler<CheckUserQueryRequest, CheckUserResponseDto>
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<AppRole> _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public CheckUserQueryRequestHandlers(IMapper mapper, IRepository<AppRole> roleRepository, IRepository<AppUser> userRepository, IUow uow)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _uow = uow;
        }

        public async Task<CheckUserResponseDto> Handle(CheckUserQueryRequest request, CancellationToken cancellationToken)
        {
            var dto = new CheckUserResponseDto();
            var user = await _uow.GetRepository<AppUser>().GetByFilterAsync(x=>x.Email == request.Email && x.Password == request.Password);
            if (user != null)
            {
                var role = await _uow.GetRepository<AppRole>().GetByIdAsync(user.AppRoleId);
                dto.Role = role?.Definition;
                dto.IsExist = true;
                dto.UserName = user.UserName;
                dto.Id = user.Id;
            }
            else
            {
                dto.IsExist = false;
                
            }
            return dto;

        }
    }
}

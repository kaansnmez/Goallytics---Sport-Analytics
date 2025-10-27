using AutoMapper;
using GoallyticsApp.Application.Features.CQRS.Commands;
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
    public class UpdateUserCommandRequestHandler : IRequestHandler<UpdateUserCommandRequest>
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;
        public UpdateUserCommandRequestHandler(IRepository<AppUser> userRepository, IMapper mapper, IUow uow)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Unit> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var selectedUser = await _uow.GetRepository<AppUser>().GetByIdAsync(request.Id);
            if (selectedUser != null)
            {
                _mapper.Map<AppUser>(selectedUser);
                await _uow.GetRepository<AppUser>().UpdateAsync(selectedUser);
                await _uow.SaveChangesAsync();
                return Unit.Value;
            }
            throw new Exception("CategoryNotFound");
        }
    }
}

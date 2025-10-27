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
    public class DeleteUserCommandRequestHandler : IRequestHandler<DeleteUserCommandRequest>
    {
        
        private readonly IRepository<AppUser> _userRepository;
        private readonly IUow _uow;
        public DeleteUserCommandRequestHandler(IRepository<AppUser> userRepository, IUow uow)
        {
            _userRepository = userRepository;
            _uow = uow;
        }

        public async Task<Unit> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
        {
            var selectedUser = await _uow.GetRepository<AppUser>().GetByIdAsync(request.Id);
            if (selectedUser == null)
            {
                await _uow.GetRepository<AppUser>().DeleteAsync(selectedUser);
                await _uow.SaveChangesAsync();
                return Unit.Value;
            }
            throw new Exception("Category not found");
        }
    }
}

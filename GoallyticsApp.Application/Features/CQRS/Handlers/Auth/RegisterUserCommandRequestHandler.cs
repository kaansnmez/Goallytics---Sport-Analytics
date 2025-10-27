using GoallyticsApp.Application.Enums;
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
    public class RegisterUserCommandRequestHandler : IRequestHandler<RegisterUserCommandRequest>
    {
        private readonly IRepository<AppUser> _userRepository;
        private readonly IUow _uow;
        public RegisterUserCommandRequestHandler(IRepository<AppUser> userRepository, IUow uow)
        {
            _userRepository = userRepository;
            _uow = uow;
        }

        public async Task<Unit> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _uow.GetRepository<AppUser>().CreateAsync(new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                DateOfBirth = request.DateOfBirth,
                AppRoleId = request.AppRoleId,
                GenderId = request.GenderId
            });
            await _uow.SaveChangesAsync();
            var uid = await _uow.GetRepository<AppUser>().GetByFilterAsync(f => f.Email == request.Email);
            
            await _uow.GetRepository<AppUserRoles>().CreateAsync(new AppUserRoles
            {
                AppUserId = uid.Id,
                AppRoleId = request.AppRoleId
            });
            await _uow.SaveChangesAsync();
            return Unit.Value;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Commands
{
    public class DeleteUserCommandRequest : IRequest
    {
        public int Id { get; set; }
        public DeleteUserCommandRequest(int id) {
            Id = Id;
        }
    }
}

using GoallyticsApp.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Queries
{
    public class GetAllGendersQueryRequest : IRequest<List<GenderDto>>
    {

    }
}

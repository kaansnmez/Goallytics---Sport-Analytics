using GoallyticsApp.Application.Features.CQRS.Queries.Statistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace GoallyticsApp.API.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StatisticController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetStatisticsByPeriod([FromQuery]GetAllStatisticQueryRequest request)
        {
            // Placeholder for actual statistics retrieval logic
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

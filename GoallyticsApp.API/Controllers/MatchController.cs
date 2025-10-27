using GoallyticsApp.Application.Features.CQRS.Queries;
using GoallyticsApp.Application.Features.CQRS.Queries.Match;
using GoallyticsApp.Application.Features.CQRS.Queries.Predictions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoallyticsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<MatchController>

        [HttpPost("[action]")]
        public async Task<IActionResult> ListFixtures(GetAllFixturesQueryRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetLeauges()
        {
            var result = await _mediator.Send(new GetLeaguesQueryRequest());
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GetStatsById(GetDetailsByTeamIdQueryRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GetPredictionsById(GetPredictionsByIdQueryRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }



    }
}

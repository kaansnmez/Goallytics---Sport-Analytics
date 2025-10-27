using GoallyticsApp.Application.Features.CQRS.Commands;
using GoallyticsApp.Application.Features.CQRS.Queries;
using GoallyticsApp.Application.Tools;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoallyticsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<AuthController>
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterUserCommandRequest request)
        {
            await _mediator.Send(request);
            return Created("", request);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(CheckUserQueryRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.IsExist)
            {
                return Created("", JwtTokenGenerator.GenerateToken(result));
            }
            return BadRequest("Password or Username is wrong.");
        }


        
        
    }
}

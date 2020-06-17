using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unidash.Auth.Application.Features.Users.Requests;

namespace Unidash.Auth.Application.Controllers
{
    [Route("connect")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConnectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public Task<IActionResult> Register([FromBody] RegisterUserRequest request) => _mediator.Send(request);

        [HttpPost("login")]
        public Task<IActionResult> Login([FromBody] LoginUserRequest request) => _mediator.Send(request);
    }
}
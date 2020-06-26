using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Unidash.Auth.Application.Features.Users.Requests;
using Unidash.Auth.Application.Features.Users.Requests.Responses;

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

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Register([FromBody] RegisterUserRequest request) => _mediator.Send(request);

        /// <summary>
        /// Logins the user with the provided credentials and returns
        /// an access token that can be used for other services.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Login([FromBody] LoginUserRequest request) => _mediator.Send(request);
    }
}
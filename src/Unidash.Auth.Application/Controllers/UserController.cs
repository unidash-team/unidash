using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unidash.Auth.Application.Features.Users.Requests;
using Unidash.Auth.Application.Features.Users.Requests.Responses;

namespace Unidash.Auth.Application.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a single user based on the user ID extracted from the JWT token.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("@me")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public Task<IActionResult> GetUser() => _mediator.Send(new GetUserRequest());
    }
}

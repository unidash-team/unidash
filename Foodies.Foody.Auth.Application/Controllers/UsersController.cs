using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Auth.Users.Requests;
using Foodies.Foody.Core.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Foody.Auth.Application.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get() => Ok(await _userEntityRepository.FindAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) => await _mediator.Send(new GetUserRequest(id));
    }
}
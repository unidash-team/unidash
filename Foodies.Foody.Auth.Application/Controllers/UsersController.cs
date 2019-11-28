using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Foody.Auth.Application.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IEntityRepository<User> _userEntityRepository;

        // TODO: Mediator pattern
        public UsersController(IEntityRepository<User> userEntityRepository)
        {
            _userEntityRepository = userEntityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _userEntityRepository.FindAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) => Ok(await _userEntityRepository.FindByIdAsync(id));
    }
}
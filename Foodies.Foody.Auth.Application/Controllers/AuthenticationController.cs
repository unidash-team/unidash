using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using Foodies.Foody.Auth.Commands;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Core.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Foodies.Foody.Auth.Application.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public static Dictionary<string, string> ProviderMapping => new Dictionary<string, string>
        {
            { "discord", DiscordAuthenticationDefaults.AuthenticationScheme }
        };

        [HttpGet("challenge/{provider}")]
        [AllowAnonymous]
        public async Task Challenge(string provider)
        {
            // TODO: Challenge command
            if (!ProviderMapping.ContainsKey(provider))
                throw new Exception("Provider does not exist");

            await HttpContext.ChallengeAsync(ProviderMapping[provider], new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Finalize))
            });
        }

        [HttpGet("finalize")]
        public async Task<IActionResult> Finalize()
        {
            // TODO: Finalize command
            if (User.Identity == null)
                return await Task.FromResult(BadRequest("User did not finish sign-in"));

            // Get claims
            var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            await _mediator.Send(new CreateUserCommand(id, name, null));

            return await Task.FromResult(Ok(new { User.Identity.AuthenticationType, id, name }));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
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
        public static Dictionary<string, string> ProviderMapping => new Dictionary<string, string>
        {
            { "discord", DiscordAuthenticationDefaults.AuthenticationScheme }
        };

        [HttpGet("challenge/{provider}")]
        [AllowAnonymous]
        public async Task<IActionResult> Challenge(string provider)
        {
            if (!ProviderMapping.ContainsKey(provider))
                return await Task.FromResult(BadRequest("Provider does not exist"));

            await HttpContext.ChallengeAsync(provider, new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Finalize))
            });

            return await Task.FromResult(Ok());
        }

        [HttpGet("finalize")]
        public async Task<IActionResult> Finalize()
        {
            var user = HttpContext.User;
            if (user.Identity == null)
                return await Task.FromResult(BadRequest("User did not finish sign-in"));

            return await Task.FromResult(Ok(HttpContext.User.Identities.First().Claims.Select(x => x.Value)));
        }
    }
}
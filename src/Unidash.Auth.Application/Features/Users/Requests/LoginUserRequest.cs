using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unidash.Auth.Application.DataModels;
using Unidash.Core.Security;

namespace Unidash.Auth.Application.Features.Users.Requests
{
    public class LoginUserRequest : IRequest<IActionResult>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public class Handler : IRequestHandler<LoginUserRequest, IActionResult>
        {
            private readonly UserManager<User> _userManager;
            private readonly JwtTokenService _jwtTokenService;

            public Handler(UserManager<User> userManager, JwtTokenService jwtTokenService)
            {
                _userManager = userManager;
                _jwtTokenService = jwtTokenService;
            }

            public async Task<IActionResult> Handle(LoginUserRequest request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == request.Email, cancellationToken);

                if (user is null)
                    return new NotFoundResult();

                var result = await _userManager.CheckPasswordAsync(user, request.Password);

                if (result)
                {
                    var token = _jwtTokenService.WriteToken(new Dictionary<string, string>
                    {
                        {ClaimTypes.NameIdentifier, user.Id.ToString()},
                        {ClaimTypes.Name, user.UserName}
                    }, new JwtTokenMeta(DateTime.UtcNow.AddDays(1), "unidash", "unidash"));

                    return new OkObjectResult(token);
                }

                return new BadRequestObjectResult("Email or password is incorrect");
            }
        }
    }
}
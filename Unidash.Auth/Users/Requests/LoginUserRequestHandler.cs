using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Unidash.Auth.Users.Queries;
using Unidash.Core.Security;

namespace Unidash.Auth.Users.Requests
{
    public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest, IActionResult>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly JwtTokenService _jwtTokenService;
        private readonly PasswordService _passwordService;

        public LoginUserRequestHandler(IMediator mediator, IMapper mapper,
            JwtTokenService jwtTokenService, PasswordService passwordService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
            _passwordService = passwordService;
        }

        public async Task<IActionResult> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            // Get user and check whether password is correct
            var userMatch = await _mediator.Send(new FindUserByEmailQuery(request.EmailAddress), cancellationToken);

            if (userMatch == null)
                return new NotFoundObjectResult("User not found");

            // Validate password
            if (!_passwordService.Validate(request.Password, userMatch.PasswordSalt, userMatch.Password))
                return new BadRequestObjectResult("Password does not match");

            // Return JWT token
            var token = _jwtTokenService.WriteToken(new Dictionary<string, string>
            {
                {ClaimTypes.Name, userMatch.Id},
                {ClaimTypes.Role, userMatch.Role ?? "Default" }
            }, new JwtTokenMeta(DateTime.UtcNow.AddDays(30), "unidash", "unidash"));

            return new OkObjectResult(new { token });
        }
    }
}
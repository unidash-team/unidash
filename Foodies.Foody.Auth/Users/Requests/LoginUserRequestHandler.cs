using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Auth.Users.Queries;
using Foodies.Foody.Core.Infrastructure;
using Foodies.Foody.Core.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Foody.Auth.Users.Requests
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
                {ClaimTypes.NameIdentifier, userMatch.Id}
            }, new JwtTokenMeta(DateTime.UtcNow.AddDays(30), "foody", "foody"));

            return new OkObjectResult(token);
        }
    }
}
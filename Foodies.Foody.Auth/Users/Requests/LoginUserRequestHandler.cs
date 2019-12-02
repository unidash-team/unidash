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

        public LoginUserRequestHandler(IMediator mediator, IMapper mapper,
            JwtTokenService jwtTokenService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<IActionResult> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            // Get user and check whether password is correct
            var userMatch = await _mediator.Send(new FindUserByEmailQuery(request.EmailAddress), cancellationToken);
            
            if (userMatch == null)
                return new NotFoundObjectResult("User not found");

            // TODO: By lord, use PasswordService and check hash
            if (request.Password != userMatch.Password)
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
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unidash.Auth.Application.Database;
using Unidash.Auth.Application.Features.Users.Requests.Responses;
using Unidash.Core.Utilities;

namespace Unidash.Auth.Application.Features.Users.Requests
{
    public class GetUserRequest : IRequest<IActionResult>
    {
        public class Handler : IRequestHandler<GetUserRequest, IActionResult>
        {
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly AuthDbContext _authDbContext;
            private readonly IMapper _mapper;

            public Handler(ICurrentUserAccessor currentUserAccessor, AuthDbContext authDbContext,
                IMapper mapper)
            {
                _currentUserAccessor = currentUserAccessor;
                _authDbContext = authDbContext;
                _mapper = mapper;
            }

            public async Task<IActionResult> Handle(GetUserRequest request, CancellationToken cancellationToken)
            {
                var userId = _currentUserAccessor.GetUserId();
                var user = await _authDbContext.Users.FindAsync(userId);

                var response = _mapper.Map<UserResponse>(user);
                return new OkObjectResult(response);
            }
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Unidash.Auth.Users.Commands;
using Unidash.Auth.Users.Queries;
using Unidash.Core.Security;

namespace Unidash.Auth.Users.Requests
{
    public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, IActionResult>
    {
        private readonly IMediator _mediator;
        private readonly PasswordService _passwordService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteUserRequestHandler(IMediator mediator, PasswordService passwordService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _passwordService = passwordService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            // Get and check user
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;

            var user = await _mediator.Send(new FindUserQuery(userId), cancellationToken);
            if (user == null)
                return new NotFoundObjectResult("User not found");

            // Verify password
            if (!_passwordService.Validate(request.Password, user.PasswordSalt, user.Password))
                return new BadRequestObjectResult("Wrong password");

            // Erase user for ever
            await _mediator.Send(new DeleteUserCommand(user.Id), cancellationToken);
            return new OkResult();
        }
    }
}
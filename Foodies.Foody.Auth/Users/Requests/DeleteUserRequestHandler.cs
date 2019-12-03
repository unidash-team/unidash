using System.Threading;
using System.Threading.Tasks;
using Foodies.Foody.Auth.Users.Commands;
using Foodies.Foody.Auth.Users.Queries;
using Foodies.Foody.Core.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Foody.Auth.Users.Requests
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
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unidash.Auth.Users.Requests
{
    public class RegisterUserRequest : IRequest<IActionResult>
    {
        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }
    }
}

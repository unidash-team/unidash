using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unidash.Auth.Users.Requests
{
    public class LoginUserRequest : IRequest<IActionResult>
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}
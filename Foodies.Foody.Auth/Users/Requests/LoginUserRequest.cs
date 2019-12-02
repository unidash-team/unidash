using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Foody.Auth.Users.Requests
{
    public class LoginUserRequest : IRequest<IActionResult>
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}
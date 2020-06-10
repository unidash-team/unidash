using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unidash.Auth.Users.Requests
{
    public class DeleteUserRequest : IRequest<IActionResult>
    {
        public string Password { get; set; }

        public DeleteUserRequest()
        {
        }

        public DeleteUserRequest(string password)
        {
            Password = password;
        }
    }
}

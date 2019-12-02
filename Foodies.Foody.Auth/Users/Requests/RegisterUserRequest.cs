using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Misc;

namespace Foodies.Foody.Auth.Users.Requests
{
    public class RegisterUserRequest : IRequest<IActionResult>
    {
        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }
    }
}

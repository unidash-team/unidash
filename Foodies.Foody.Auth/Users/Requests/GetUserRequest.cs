using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Foody.Auth.Users.Requests
{
    public class GetUserRequest : IRequest<IActionResult>
    {
        public string Id { get; set; }

        public GetUserRequest(string id)
        {
            Id = id;
        }
    }
}

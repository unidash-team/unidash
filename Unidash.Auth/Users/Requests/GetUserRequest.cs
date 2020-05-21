using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unidash.Auth.Users.Requests
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

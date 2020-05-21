using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Unidash.Auth.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public string Id { get; set; }

        public DeleteUserCommand(string id)
        {
            Id = id;
        }
    }
}

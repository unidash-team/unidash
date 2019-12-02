using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Foodies.Foody.Auth.Commands
{
    public class CreateUserCommand : IRequest
    {
        public string Id { get; private set; }

        public string DisplayName { get; private set; }

        public string EmailAddress { get; private set; }

        public string Password { get; private set; }

        public CreateUserCommand(string id, string displayName, string emailAddress, string password)
        {
            Id = id;
            DisplayName = displayName;
            EmailAddress = emailAddress;
            Password = password;
        }
    }
}

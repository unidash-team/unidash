using System;
using System.Collections.Generic;
using System.Text;
using Foodies.Foody.Auth.Domain.UserAggregate;
using MediatR;

namespace Foodies.Foody.Auth.Users.Queries
{
    public class FindUserByEmailQuery : IRequest<User>
    {
        public string EmailAddress { get; private set; }

        public FindUserByEmailQuery(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}

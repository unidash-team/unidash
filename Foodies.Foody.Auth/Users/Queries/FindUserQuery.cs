using System;
using System.Collections.Generic;
using System.Text;
using Foodies.Foody.Auth.Domain.UserAggregate;
using MediatR;

namespace Foodies.Foody.Auth.Users.Queries
{
    public class FindUserQuery : IRequest<User>
    {
        public string Id { get; set; }

        public FindUserQuery(string id)
        {
            Id = id;
        }
    }
}

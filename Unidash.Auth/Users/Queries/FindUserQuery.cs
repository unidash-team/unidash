using System;
using System.Collections.Generic;
using System.Text;
using Unidash.Auth.Domain.UserAggregate;
using MediatR;

namespace Unidash.Auth.Users.Queries
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

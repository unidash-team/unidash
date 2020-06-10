using MediatR;
using Unidash.Auth.Domain.UserAggregate;

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

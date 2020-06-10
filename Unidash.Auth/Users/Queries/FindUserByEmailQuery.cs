using MediatR;
using Unidash.Auth.Domain.UserAggregate;

namespace Unidash.Auth.Users.Queries
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

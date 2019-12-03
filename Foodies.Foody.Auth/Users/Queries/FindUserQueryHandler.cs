using System.Threading;
using System.Threading.Tasks;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Core.Infrastructure;
using MediatR;

namespace Foodies.Foody.Auth.Users.Queries
{
    public class FindUserQueryHandler : IRequestHandler<FindUserQuery, User>
    {
        private readonly IEntityRepository<User> _repository;

        public FindUserQueryHandler(IEntityRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User> Handle(FindUserQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindByIdAsync(request.Id);
        }
    }
}
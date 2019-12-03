using System.Threading;
using System.Threading.Tasks;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Core.Infrastructure;
using MediatR;

namespace Foodies.Foody.Auth.Users.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IEntityRepository<User> _repository;

        public DeleteUserCommandHandler(IEntityRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _repository.RemoveByIdAsync(request.Id);
            return Unit.Value;
        }
    }
}
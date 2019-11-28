using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Core.Infrastructure;
using MediatR;

namespace Foodies.Foody.Auth.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IEntityRepository<User> _repository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IEntityRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            var entity = await _repository.GetOrCreateAsync(user.Id, user);

            return Unit.Value;
            // return _mapper.Map<UserDto>(entity);
        }
    }
}
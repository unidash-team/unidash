using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Core.Infrastructure;
using Foodies.Foody.Core.Security;
using MediatR;

namespace Foodies.Foody.Auth.Users.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IEntityRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;

        public CreateUserCommandHandler(IEntityRepository<User> repository, IMapper mapper,
            PasswordService passwordService)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            user.PasswordSalt = _passwordService.GenerateRandomSaltAsBase64();
            user.Password = _passwordService.Hash(user.Password, user.PasswordSalt);

            await _repository.GetOrCreateAsync(user.Id, user);

            return Unit.Value;
        }
    }
}
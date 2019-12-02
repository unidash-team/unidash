﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Foodies.Foody.Auth.Domain.UserAggregate;
using Foodies.Foody.Core.Infrastructure;
using MediatR;

namespace Foodies.Foody.Auth.Users.Queries
{
    public class FindUserByEmailQueryHandler : IRequestHandler<FindUserByEmailQuery, User>
    {
        private readonly IEntityRepository<User> _repository;

        public FindUserByEmailQueryHandler(IEntityRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.FindAllByAsync(u => u.EmailAddress == request.EmailAddress);
            return list.SingleOrDefault();
        }
    }
}
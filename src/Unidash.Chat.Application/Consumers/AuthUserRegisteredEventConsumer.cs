using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Unidash.Chat.Application.DataModels;
using Unidash.Core.Infrastructure;
using Unidash.Events.Auth;

namespace Unidash.Chat.Application.Consumers
{
    public class AuthUserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly IEntityRepository<ChatUser> _userEntityRepository;

        public AuthUserRegisteredEventConsumer(IEntityRepository<ChatUser> userEntityRepository)
        {
            _userEntityRepository = userEntityRepository;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            await _userEntityRepository.AddAsync(new ChatUser
            {
                Id = context.Message.Id,
                DisplayName = context.Message.DisplayName
            });
        }
    }
}

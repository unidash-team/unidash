using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.SignalR;
using Unidash.Chat.Application.DataModels;
using Unidash.Chat.Application.Hubs;
using Unidash.Core.Infrastructure;
using Unidash.Core.Utilities;

namespace Unidash.Chat.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IEntityRepository<ChatMessage> _chatMessageEntityRepository;
        private readonly IEntityRepository<ChatChannel> _chatChannelEntityRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHubContext<ChatHub, IChatHubClient> _hubContext;

        public MessageService(IEntityRepository<ChatMessage> chatMessageEntityRepository,
            IEntityRepository<ChatChannel> chatChannelEntityRepository,
            ICurrentUserAccessor currentUserAccessor, IHubContext<ChatHub, IChatHubClient> hubContext)
        {
            _chatMessageEntityRepository = chatMessageEntityRepository;
            _chatChannelEntityRepository = chatChannelEntityRepository;
            _currentUserAccessor = currentUserAccessor;
            _hubContext = hubContext;
        }

        public async Task SendAsync(string channelId, string message)
        {
            var userId = _currentUserAccessor.GetUserId();
            Ensure.That(userId).IsNotNullOrEmpty();

            var channel = await GetChannelAsync(channelId);
            Ensure.That(channel).IsNotNull();

            var chatMessage = new ChatMessage
            {
                ChannelId = channelId,
                Message = message,
                UserId = userId
            };

            await _chatMessageEntityRepository.AddAsync(chatMessage);

            await _hubContext.Clients.Users(channel.ParticipantUserIds.ToList())
                .ReceiveMessage(chatMessage);
        }

        public Task<ChatChannel> GetChannelAsync(string channelId) => _chatChannelEntityRepository.FindAsync(channelId);
    }
}
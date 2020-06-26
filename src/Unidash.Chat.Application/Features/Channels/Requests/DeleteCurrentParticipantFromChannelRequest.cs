using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unidash.Chat.Application.DataModels;
using Unidash.Core.Infrastructure;
using Unidash.Core.Utilities;

namespace Unidash.Chat.Application.Features.Channels.Requests
{
    public class DeleteCurrentParticipantFromChannelRequest : IRequest<IActionResult>
    {
        public string ChannelId { get; set; }

        public DeleteCurrentParticipantFromChannelRequest(string channelId)
        {
            ChannelId = channelId;
        }

        public class Handler : IRequestHandler<DeleteCurrentParticipantFromChannelRequest, IActionResult>
        {
            private readonly IEntityRepository<ChatChannel> _chatChannelEntityRepository;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(IEntityRepository<ChatChannel> chatChannelEntityRepository, ICurrentUserAccessor currentUserAccessor)
            {
                _chatChannelEntityRepository = chatChannelEntityRepository;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<IActionResult> Handle(DeleteCurrentParticipantFromChannelRequest request, CancellationToken cancellationToken)
            {
                var userId = _currentUserAccessor.GetUserId();

                var channel = await _chatChannelEntityRepository.FindAsync(request.ChannelId);
                if (!channel.ParticipantUserIds.Contains(userId))
                    return new UnauthorizedResult();

                channel.ParticipantUserIds.Remove(userId);
                await _chatChannelEntityRepository.UpdateAsync(channel);

                return new OkResult();
            }
        }
    }
}
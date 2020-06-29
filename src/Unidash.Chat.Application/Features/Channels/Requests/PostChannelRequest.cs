using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unidash.Chat.Application.DataModels;
using Unidash.Core.Infrastructure;
using Unidash.Core.Utilities;

namespace Unidash.Chat.Application.Features.Channels.Requests
{
    public class PostChannelRequest : IRequest<IActionResult>
    {
        public string Title { get; set; } = "Untitled Conversation";

        public ICollection<string> Participants { get; set; } = new HashSet<string>();

        public class Handler : IRequestHandler<PostChannelRequest, IActionResult>
        {
            private readonly IEntityRepository<ChatChannel> _channelEntityRepository;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(IEntityRepository<ChatChannel> channelEntityRepository,
                ICurrentUserAccessor currentUserAccessor)
            {
                _channelEntityRepository = channelEntityRepository;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<IActionResult> Handle(PostChannelRequest request, CancellationToken cancellationToken)
            {
                var userId = _currentUserAccessor.GetUserId();
                request.Participants.Add(userId);

                var channel = new ChatChannel
                {
                    Title = request.Title,
                    ParticipantUserIds = request.Participants,
                    CreatorUserId = userId
                };

                // TODO: Check whether all participants even exist in DB

                await _channelEntityRepository.AddAsync(channel);

                return new OkResult();
            }
        }
    }
}
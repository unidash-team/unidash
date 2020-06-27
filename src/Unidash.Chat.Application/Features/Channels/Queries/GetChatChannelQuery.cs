using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EnsureThat;
using MediatR;
using Unidash.Chat.Application.DataModels;
using Unidash.Chat.Application.DataTransfer;
using Unidash.Chat.Application.DataTransfer.Partials;
using Unidash.Core.Infrastructure;

namespace Unidash.Chat.Application.Features.Channels.Queries
{
    /// <summary>
    /// Returns a <see cref="ChatChannelResponse"/> of the channel with the associated ID.
    /// </summary>
    public class GetChatChannelQuery : IRequest<ChatChannelResponse>
    {
        public string Id { get; set; }

        public GetChatChannelQuery(string id)
        {
            Id = id;
        }

        public class Handler : IRequestHandler<GetChatChannelQuery, ChatChannelResponse>
        {
            private readonly IEntityRepository<ChatChannel> _chatChannelEntityRepository;
            private readonly IEntityRepository<ChatUser> _chatUserEntityRepository;
            private readonly IMapper _mapper;

            public Handler(IEntityRepository<ChatChannel> chatChannelEntityRepository,
                IEntityRepository<ChatUser> chatUserEntityRepository,
                IMapper mapper)
            {
                _chatChannelEntityRepository = chatChannelEntityRepository;
                _chatUserEntityRepository = chatUserEntityRepository;
                _mapper = mapper;
            }

            public async Task<ChatChannelResponse> Handle(GetChatChannelQuery request, CancellationToken cancellationToken)
            {
                var channel = await _chatChannelEntityRepository.FindAsync(request.Id);
                Ensure.That(channel).IsNotNull();

                var response = _mapper.Map<ChatChannelResponse>(channel);

                // Get participants
                foreach (var participantUserId in channel.ParticipantUserIds)
                {
                    // TODO: Optimize with user list query + parameters
                    var user = await _chatUserEntityRepository.FindAsync(participantUserId);
                    if (user is null)
                        continue;

                    var userResponse = _mapper.Map<ChatUserPartialResponse>(user);
                    response.Participants.Add(userResponse);
                }

                return response;
            }
        }
    }
}

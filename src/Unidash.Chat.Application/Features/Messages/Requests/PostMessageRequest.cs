using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Unidash.Chat.Application.DataModels;
using Unidash.Chat.Application.Hubs;
using Unidash.Core.Infrastructure;
using Unidash.Core.Utilities;

namespace Unidash.Chat.Application.Features.Messages.Requests
{
    public class PostMessageRequest : IRequest<IActionResult>
    {
        public string ChannelId { get; set; }

        public string Message { get; set; }

        public PostMessageRequest(string channelId, string message)
        {
            ChannelId = channelId;
            Message = message;
        }

        public class Handler : IRequestHandler<PostMessageRequest, IActionResult>
        {
            private readonly ICurrentUserAccessor _userAccessor;
            private readonly IEntityRepository<ChatMessage> _messageEntityRepository;
            private readonly IHubContext<ChatHub> _chatHubContext;

            public Handler(ICurrentUserAccessor userAccessor,
                IEntityRepository<ChatMessage> messageEntityRepository,
                IHubContext<ChatHub> chatHubContext)
            {
                _userAccessor = userAccessor;
                _messageEntityRepository = messageEntityRepository;
                _chatHubContext = chatHubContext;
            }

            public async Task<IActionResult> Handle(PostMessageRequest request, CancellationToken cancellationToken)
            {
                var message = new ChatMessage
                {
                    Message = request.Message,
                    ChannelId = request.ChannelId,
                    UserId = _userAccessor.GetUserId()
                };

                await _messageEntityRepository.AddAsync(message);

                return new OkObjectResult(message);
            }
        }
    }
}

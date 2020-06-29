using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Unidash.Chat.Application.DataModels;
using Unidash.Chat.Application.Hubs;
using Unidash.Chat.Application.Services;
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
            private readonly IMessageService _messageService;

            public Handler(IMessageService messageService)
            {
                _messageService = messageService;
            }

            public async Task<IActionResult> Handle(PostMessageRequest request, CancellationToken cancellationToken)
            {
                await _messageService.SendAsync(request.ChannelId, request.Message);

                return new OkResult();
            }
        }
    }
}

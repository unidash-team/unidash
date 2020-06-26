using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unidash.Chat.Application.Features.Messages.Queries;
using Unidash.Core.Utilities;

namespace Unidash.Chat.Application.Features.Messages.Requests
{
    public class GetMessageListRequest : IRequest<IActionResult>
    {
        public string ChannelId { get; set; }

        public GetMessageListRequest(string channelId)
        {
            ChannelId = channelId;
        }

        public class Handler : IRequestHandler<GetMessageListRequest, IActionResult>
        {
            private readonly IMediator _mediator;
            private readonly ICurrentUserAccessor _userAccessor;

            public Handler(IMediator mediator, ICurrentUserAccessor userAccessor)
            {
                _mediator = mediator;
                _userAccessor = userAccessor;
            }

            public async Task<IActionResult> Handle(GetMessageListRequest request, CancellationToken cancellationToken)
            {
                var query = new ListChatMessagesQuery
                {
                    ChannelId = request.ChannelId,
                    UserId = _userAccessor.GetUserId()
                };

                var result = await _mediator.Send(query, cancellationToken);

                return new OkObjectResult(result);
            }
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unidash.Chat.Application.Features.Channels.Queries;

namespace Unidash.Chat.Application.Features.Channels.Requests
{
    public class GetChannelRequest : IRequest<IActionResult>
    {
        public string Id { get; set; }

        public GetChannelRequest(string id)
        {
            Id = id;
        }

        public class Handler : IRequestHandler<GetChannelRequest, IActionResult>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<IActionResult> Handle(GetChannelRequest request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetChatChannelQuery(request.Id), cancellationToken);
                return new OkObjectResult(response);
            }
        }
    }
}
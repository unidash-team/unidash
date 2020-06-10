using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Unidash.TimeTable.Models.Queries;

namespace Unidash.TimeTable.Requests
{
    public class GetCalendarEntryRequest : IRequest<IActionResult>
    {
        public string Id { get; set; }

        public GetCalendarEntryRequest(string id)
        {
            Id = id;
        }

        public class Handler : IRequestHandler<GetCalendarEntryRequest, IActionResult>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<IActionResult> Handle(GetCalendarEntryRequest request, CancellationToken cancellationToken)
            {
                var entry = await _mediator.Send(new GetCalendarEntryQuery(request.Id), cancellationToken);

                if (entry is null)
                    return new NotFoundResult();

                return new OkObjectResult(entry);
            }
        }
    }
}
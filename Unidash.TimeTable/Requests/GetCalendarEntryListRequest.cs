using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Unidash.TimeTable.Models.Queries;
using Unidash.TimeTable.Models.Queries.Parameters;

namespace Unidash.TimeTable.Requests
{
    public class GetCalendarEntryListRequest : IRequest<IActionResult>
    {
        public CalendarEntriesQueryParameters Parameters { get; set; }

        public GetCalendarEntryListRequest(CalendarEntriesQueryParameters parameters)
        {
            Parameters = parameters;
        }

        public class Handler : IRequestHandler<GetCalendarEntryListRequest, IActionResult>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<IActionResult> Handle(GetCalendarEntryListRequest entryListRequest, CancellationToken cancellationToken)
            {
                var result = await _mediator.Send(new ListCalendarEntriesQuery(entryListRequest.Parameters), cancellationToken);

                return new OkObjectResult(result);
            }
        }
    }
}

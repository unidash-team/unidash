using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unidash.TimeTable.Models.Queries.Parameters;
using Unidash.TimeTable.Requests;

namespace Unidash.TimeTable.Application.Controllers
{
    [Route("calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("@all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> GetByQuery([FromQuery] CalendarEntriesQueryParameters parameters) =>
            _mediator.Send(new GetCalendarEntryListRequest(parameters));

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Get(string id) => _mediator.Send(new GetCalendarEntryRequest(id));
    }
}

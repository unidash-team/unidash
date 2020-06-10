using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unidash.TimeTable.Models.Queries.Parameters;
using Unidash.TimeTable.Requests.DataTransfer;

namespace Unidash.TimeTable.Models.Queries
{
    public class ListCalendarEntriesQuery : IRequest<IList<CalendarEntry>>
    {
        public CalendarEntriesQueryParameters Parameters { get; set; }

        public ListCalendarEntriesQuery(CalendarEntriesQueryParameters parameters)
        {
            Parameters = parameters;
        }

        public class Handler : IRequestHandler<ListCalendarEntriesQuery, IList<CalendarEntry>>
        {
            private readonly TimeTableDbContext _timeTableDbContext;
            private readonly IMapper _mapper;

            public Handler(TimeTableDbContext timeTableDbContext, IMapper mapper)
            {
                _timeTableDbContext = timeTableDbContext;
                _mapper = mapper;
            }

            public async Task<IList<CalendarEntry>> Handle(ListCalendarEntriesQuery request, CancellationToken cancellationToken)
            {
                var query = _timeTableDbContext.CalendarEntries.AsQueryable();

                if (request.Parameters.From != null)
                    query = query.Where(x => request.Parameters.From >= x.StartsAt);

                if (request.Parameters.To != null)
                    query = query.Where(x => x.EndsAt <= request.Parameters.To);

                var result = await query.ToListAsync(cancellationToken);
                return _mapper.Map<IList<CalendarEntry>>(result);
            }
        }
    }
}

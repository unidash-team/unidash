using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Unidash.TimeTable.Requests.DataTransfer;

namespace Unidash.TimeTable.Models.Queries
{
    public class GetCalendarEntryQuery : IRequest<CalendarEntry>
    {
        public string Id { get; set; }

        public GetCalendarEntryQuery(string id)
        {
            Id = id;
        }

        public class Handler : IRequestHandler<GetCalendarEntryQuery, CalendarEntry>
        {
            private readonly TimeTableDbContext _timeTableDbContext;
            private readonly IMapper _mapper;

            public Handler(TimeTableDbContext timeTableDbContext, IMapper mapper)
            {
                _timeTableDbContext = timeTableDbContext;
                _mapper = mapper;
            }

            public async Task<CalendarEntry> Handle(GetCalendarEntryQuery request, CancellationToken cancellationToken)
            {
                var result = await _timeTableDbContext.CalendarEntries.FindAsync(request.Id);
                return _mapper.Map<CalendarEntry>(result);
            }
        }
    }
}
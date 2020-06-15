using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Unidash.Core.Infrastructure;
using Unidash.TimeTable.Models.Resources;

namespace Unidash.TimeTable.Models.Queries
{
    public class GetCalendarEntryQuery : IRequest<CalendarEventResource>
    {
        public string Id { get; set; }

        public GetCalendarEntryQuery(string id)
        {
            Id = id;
        }

        public class Handler : IRequestHandler<GetCalendarEntryQuery, CalendarEventResource>
        {
            private readonly IEntityRepository<CalendarEventEntity> _entityRepository;
            private readonly IMapper _mapper;

            public Handler(IEntityRepository<CalendarEventEntity> entityRepository, IMapper mapper)
            {
                _entityRepository = entityRepository;
                _mapper = mapper;
            }

            public async Task<CalendarEventResource> Handle(GetCalendarEntryQuery request, CancellationToken cancellationToken)
            {
                var result = await _entityRepository.FindAsync(request.Id);
                return _mapper.Map<CalendarEventResource>(result);
            }
        }
    }
}
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unidash.Core.Infrastructure;
using Unidash.TimeTable.Models.Queries.Parameters;
using Unidash.TimeTable.Models.Resources;

namespace Unidash.TimeTable.Models.Queries
{
    public class ListCalendarEntriesQuery : IRequest<IList<CalendarEventResource>>
    {
        public CalendarEntriesQueryParameters Parameters { get; set; }

        public ListCalendarEntriesQuery(CalendarEntriesQueryParameters parameters)
        {
            Parameters = parameters;
        }

        public class Handler : IRequestHandler<ListCalendarEntriesQuery, IList<CalendarEventResource>>
        {
            private readonly IEntityRepository<CalendarEventEntity> _entityRepository;
            private readonly IMapper _mapper;

            public Handler(IEntityRepository<CalendarEventEntity> entityRepository, IMapper mapper)
            {
                _entityRepository = entityRepository;
                _mapper = mapper;
            }

            public async Task<IList<CalendarEventResource>> Handle(ListCalendarEntriesQuery request, CancellationToken cancellationToken)
            {
                var query = _entityRepository.AsQueryable();

                if (!request.Parameters.IncludeHidden)
                    query = query.Where(x => x.IsHidden == false);

                if (request.Parameters.From != null)
                    query = query.Where(x => request.Parameters.From >= x.StartsAt);

                if (request.Parameters.To != null)
                    query = query.Where(x => x.EndsAt <= request.Parameters.To);

                var result = query
                    .OrderBy(x => x.StartsAt)
                    .ToList();

                return _mapper.Map<IList<CalendarEventResource>>(result);
            }
        }
    }
}

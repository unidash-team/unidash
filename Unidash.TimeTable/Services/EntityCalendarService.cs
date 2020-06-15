using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unidash.Core.Infrastructure;
using Unidash.TimeTable.Models;

namespace Unidash.TimeTable.Services
{
    public class EntityCalendarService : ICalendarService
    {
        private readonly IEntityRepository<CalendarEventEntity> _entityRepository;
        private readonly IMapper _mapper;

        public EntityCalendarService(IEntityRepository<CalendarEventEntity> entityRepository, IMapper mapper)
        {
            _entityRepository = entityRepository;
            _mapper = mapper;
        }

        public async Task AddOrUpdateCalendarEntryAsync(CalendarEventEntity entity)
        {
            var isFound = (await _entityRepository.FindAsync(entity.Id)) != null;
            if (isFound)
                await UpdateCalendarEntryAsync(entity);
            else
                await AddCalendarEntryAsync(entity);
        }

        public Task<IEnumerable<CalendarEventEntity>> GetAllEventsAsync() => _entityRepository.FindAllAsync();

        public Task RemoveEventAsync(string eventId) => _entityRepository.RemoveAsync(eventId);

        public async Task AddCalendarEntryAsync(CalendarEventEntity entity)
        {
            await _entityRepository.AddAsync(entity);
        }

        public async Task UpdateCalendarEntryAsync(CalendarEventEntity entity)
        {
            // Get entity
            var result = await _entityRepository.FindAsync(entity.Id);

            // Map
            var updatedResult = _mapper.Map(result, entity);

            // Update
            await _entityRepository.UpdateAsync(updatedResult);
        }
    }
}
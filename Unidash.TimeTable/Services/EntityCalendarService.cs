using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Unidash.TimeTable.Models;

namespace Unidash.TimeTable.Services
{
    public class EntityCalendarService : ICalendarService
    {
        private readonly IMapper _mapper;
        private readonly TimeTableDbContext _timeTableDbContext;

        public EntityCalendarService(IServiceProvider serviceProvider, IMapper mapper)
        {
            _mapper = mapper;
            var scope = serviceProvider.CreateScope();
            _timeTableDbContext = scope.ServiceProvider.GetRequiredService<TimeTableDbContext>();
        }

        public async Task AddOrUpdateCalendarEntryAsync(CalendarEntryEntity entity)
        {
            var isFound = await _timeTableDbContext.CalendarEntries.FindAsync(entity.Id) != null;
            if (isFound)
                await UpdateCalendarEntryAsync(entity);
            else
                await AddCalendarEntryAsync(entity);
        }

        public async Task AddCalendarEntryAsync(CalendarEntryEntity entity)
        {
            _timeTableDbContext.CalendarEntries.Add(entity);
            await _timeTableDbContext.SaveChangesAsync();
        }

        public async Task UpdateCalendarEntryAsync(CalendarEntryEntity entity)
        {
            // Get entity
            var result = await _timeTableDbContext.CalendarEntries.FindAsync(entity.Id);

            // Map
            var updatedResult = _mapper.Map(result, entity);

            // Update
            _timeTableDbContext.CalendarEntries.Update(updatedResult);
            await _timeTableDbContext.SaveChangesAsync();
        }
    }
}
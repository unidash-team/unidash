using System.Collections.Generic;
using System.Threading.Tasks;
using Unidash.TimeTable.Models;

namespace Unidash.TimeTable.Services
{
    public interface ICalendarService
    {
        /// <summary>
        /// Adds or updates an event by its ID.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddOrUpdateCalendarEntryAsync(CalendarEventEntity entity);

        /// <summary>
        /// Returns all events.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CalendarEventEntity>> GetAllEventsAsync();

        /// <summary>
        /// Removes an event by its ID.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task RemoveEventAsync(string eventId);
    }
}

using System.Threading.Tasks;
using Unidash.TimeTable.Models;

namespace Unidash.TimeTable.Services
{
    public interface ICalendarService
    {
        Task AddOrUpdateCalendarEntryAsync(CalendarEntryEntity entity);
    }
}

using Microsoft.EntityFrameworkCore;
using Unidash.TimeTable.Models;

namespace Unidash.TimeTable
{
    public class TimeTableDbContext : DbContext
    {
        public DbSet<CalendarEntryEntity> CalendarEntries { get; set; }

        public TimeTableDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
    }
}

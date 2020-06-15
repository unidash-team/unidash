using System;

namespace Unidash.TimeTable.Models.Queries.Parameters
{
    public class CalendarEntriesQueryParameters
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public bool IncludeHidden { get; set; }
    }
}
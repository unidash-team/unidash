using System;
using Unidash.Core.Domain;

namespace Unidash.TimeTable.Models.Resources
{
    public class CalendarEventResource : EntityDto
    {
        public string Title { get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }

        public TimeSpan Duration => EndsAt.Subtract(StartsAt);

        public bool IsHidden { get; set; }
        
        public string Source { get; set; }
    }
}

using System;
using Unidash.Core.Domain;

namespace Unidash.TimeTable.Models
{
    public class CalendarEntryEntity : Entity
    {
        public string Title { get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }

        public bool IsHidden { get; set; }
    }
}

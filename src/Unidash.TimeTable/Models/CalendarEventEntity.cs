using System;
using Unidash.Core.Domain;

namespace Unidash.TimeTable.Models
{
    public class CalendarEventEntity : Entity
    {
        public string Title { get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }

        public bool IsHidden { get; set; }

        public string Source { get; set; }

        /// <summary>
        /// Provides constant defaults for <see cref="CalendarEventEntity"/>'s Source property.
        /// </summary>
        public static class SourceDefaults
        {
            public const string System = "System";
            public const string User = "User";
        }
    }
}

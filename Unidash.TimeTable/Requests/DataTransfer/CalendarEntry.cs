using System;
using Unidash.Core.Domain;

namespace Unidash.TimeTable.Requests.DataTransfer
{
    public class CalendarEntry : EntityDto
    {
        public string Title { get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }
    }
}

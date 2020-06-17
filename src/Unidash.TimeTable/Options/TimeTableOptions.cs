using System;

namespace Unidash.TimeTable.Options
{
    public class TimeTableOptions
    {
        public string UpstreamICalUrl { get; set; }

        public TimeSpan UpstreamSyncInterval { get; set; } = TimeSpan.FromMinutes(30);
    }
}

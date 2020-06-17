using AutoMapper;
using EnsureThat;
using Ical.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unidash.TimeTable.Models;
using Unidash.TimeTable.Options;
using Unidash.TimeTable.Services;

namespace Unidash.TimeTable.Workers
{

    public class CalendarSyncBackgroundWorker : IHostedService
    {
        private readonly IOptions<TimeTableOptions> _options;
        private readonly IMapper _mapper;
        private readonly ICalendarService _calendarService;
        private readonly ILogger<CalendarSyncBackgroundWorker> _logger;
        private Timer _timer;
        private readonly HttpClient _client;

        public CalendarSyncBackgroundWorker(IOptions<TimeTableOptions> options,
            IMapper mapper,
            ICalendarService calendarService,
            ILogger<CalendarSyncBackgroundWorker> logger)
        {
            _options = options;
            _mapper = mapper;
            _calendarService = calendarService;
            _logger = logger;
            _client = new HttpClient();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Ensure.That(_options.Value.UpstreamICalUrl).IsNotEmptyOrWhiteSpace();

            _timer = new Timer(Run, null, TimeSpan.Zero, _options.Value.UpstreamSyncInterval);

            return Task.CompletedTask;
        }

        public async void Run(object state)
        {
            var response = await _client.GetAsync(_options.Value.UpstreamICalUrl);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var calendars = CalendarCollection.Load(stream);
            var calendar = calendars.FirstOrDefault();

            if (calendar is null)
            {
                _logger.LogWarning("Calendar is empty. Skipping sync");
                return;
            }

            // Add or update events
            var upstreamCalendarEvents = calendar.Events.OrderBy(x => x.Start).ToList();
            foreach (var calendarEvent in upstreamCalendarEvents)
            {
                var entry = _mapper.Map<CalendarEventEntity>(calendarEvent);
                entry.Source = _options.Value.UpstreamICalUrl;

                await _calendarService.AddOrUpdateCalendarEntryAsync(entry);
            }

            // Purge obsolete events
            var events = (await _calendarService.GetAllEventsAsync()).ToList();
            var obsoleteEventIds = events.Where(x => x.Source == _options.Value.UpstreamICalUrl)
                .Select(x => x.Id)
                .Except(upstreamCalendarEvents
                    .Select(x => x.Uid));

            // TODO: Add tests
            foreach (var obsoleteEventId in obsoleteEventIds)
            {
                await _calendarService.RemoveEventAsync(obsoleteEventId);
            }

            _logger.LogInformation("Synchronized calendar from upstream source ({UpstreamSourceUrl})",
                _options.Value.UpstreamICalUrl);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _timer.DisposeAsync();
        }
    }
}

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

            foreach (var calendarEvent in calendar.Events)
            {
                var entry = _mapper.Map<CalendarEntryEntity>(calendarEvent);
                await _calendarService.AddOrUpdateCalendarEntryAsync(entry);
            }

            _logger.LogInformation("Synchronized calendar from upstream source");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _timer.DisposeAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Foodies.Foody.Canteen.Core.Data;
using RestSharp;

namespace Foodies.Foody.Canteen.Core.Services
{
    public struct OpenMensaCanteenConfiguration
    {
        public string CanteenId { get; set; }
    }

    public class OpenMensaCanteenService : ICanteenService
    {
        private readonly OpenMensaCanteenConfiguration _configuration;
        private RestClient _client;

        public const string EndpointUrl = "https://openmensa.org/api/v2";

        public OpenMensaCanteenService(OpenMensaCanteenConfiguration configuration)
        {
            _configuration = configuration;

            _client = new RestClient(EndpointUrl);
        }

        public Task<IEnumerable<Meal>> GetMealsAsync() => GetMealsAsync(DateTime.UtcNow);

        public async Task<IEnumerable<Meal>> GetMealsAsync(DateTime date)
        {
            var request = new RestRequest("canteens/{id}/days/{date}/meals", Method.GET)
                .AddUrlSegment("id", _configuration.CanteenId)
                .AddUrlSegment("date", FormatDateTime(date));
            
            return await _client.GetAsync<List<Meal>>(request);
        }

        public async Task<Dictionary<DateTime, IEnumerable<Meal>>> GetMealsOfWeekAsync()
        {
            var dictionary = new Dictionary<DateTime, IEnumerable<Meal>>();
            var date = DateTime.Today;

            while (date.DayOfWeek <= DayOfWeek.Friday)
            {
                var meals = await GetMealsAsync(date);
                dictionary.Add(date, meals);

                date = date.AddDays(1);
            }

            return dictionary;
        }

        public string FormatDateTime(DateTime date) => date.ToString("yyyy-MM-dd");
    }
}
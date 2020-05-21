using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Unidash.Canteen.Core.Data;
using RestSharp;

namespace Unidash.Canteen.Core.Services
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

        public async Task<IEnumerable<MealsDateTuple>> GetMealsOfWeekAsync()
        {
            var list = new List<MealsDateTuple>();
            var date = DateTime.Today;

            while (date.DayOfWeek <= DayOfWeek.Friday)
            {
                var meals = await GetMealsAsync(date);
                list.Add(new MealsDateTuple(date, meals));

                date = date.AddDays(1);
            }

            return list;
        }

        public string FormatDateTime(DateTime date) => date.ToString("yyyy-MM-dd");
    }
}
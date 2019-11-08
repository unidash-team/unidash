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
            return await _client.GetAsync<List<Meal>>(
                new RestRequest($"/canteens/{_configuration.CanteenId}/days/" + 
                                $"{FormatDateTime(date)}/meals", Method.GET));
        }

        public string FormatDateTime(DateTime date) => date.ToString("YYYY-MM-DD");
    }
}
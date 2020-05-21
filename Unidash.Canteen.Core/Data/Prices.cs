using Newtonsoft.Json;

namespace Unidash.Canteen.Core.Data
{
    public class Prices
    {
        [JsonProperty("students", NullValueHandling = NullValueHandling.Ignore)]
        public double? Students { get; set; }

        [JsonProperty("employees", NullValueHandling = NullValueHandling.Ignore)]
        public double? Employees { get; set; }

        [JsonProperty("others", NullValueHandling = NullValueHandling.Ignore)]
        public double? Others { get; set; }

        [JsonProperty("pupils", NullValueHandling = NullValueHandling.Ignore)]
        public double? Pupils { get; set; }
    }
}
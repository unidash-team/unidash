using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Foodies.Foody.Canteen.Core.Data
{
    public class Meal
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("notes")]
        public List<string> Notes { get; set; }

        [JsonProperty("prices")]
        public Prices Prices { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}

using Newtonsoft.Json;

namespace MyFunctionApp
{
    public class Item
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
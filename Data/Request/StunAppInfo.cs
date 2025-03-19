using System.Text.Json.Serialization;

namespace StunPages.Data
{
    public class StunAppInfo
    {
        [JsonPropertyName("key")]
        public required string Key { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("ip")]
        public required string Ip { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("protocol")]
        public string? Protocol { get; set; }

        public string? Type { get; set; }
    }
}

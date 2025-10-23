using System.Text.Json.Serialization;

namespace APIEbeer.Shared.ViewModels.JSON
{
    public class CharacteristicViewModel
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("value")]
        public required string Value { get; set; }

        [JsonPropertyName("options")]
        public required List<string> Options { get; set; }
    }
}
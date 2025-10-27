using System.Text.Json.Serialization;

namespace GoallyticsApp.UI.Models.Statistics
{
    public class MarketsDataModel
    {
        [JsonPropertyName("market1x2")]
        public MarketCardStatsModel Market1x2 { get; set; } = new();
        [JsonPropertyName("marketOU15")]
        public MarketCardStatsModel OU15 { get; set; } = new();
        [JsonPropertyName("marketOU25")]
        public MarketCardStatsModel OU25 { get; set; } = new();
        [JsonPropertyName("marketOU35")]
        public MarketCardStatsModel OU35 { get; set; } = new();
        [JsonPropertyName("marketBTTS")]
        public MarketCardStatsModel BTTS { get; set; } = new();
        [JsonPropertyName("overall")]
        public MarketCardStatsModel Overall { get; set; } = new();
    }
}

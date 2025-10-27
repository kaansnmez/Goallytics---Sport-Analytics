using System.Text.Json.Serialization;

namespace GoallyticsApp.UI.Models.Statistics
{
    public class MarketCardStatsModel
    {
        [JsonPropertyName("total")]
        public int Total { get; set; } = 0;
        [JsonPropertyName("correct")]
        public int Correct { get; set; } = 0;
        [JsonPropertyName("accuracy")]
        public double Accuracy { get; set; } = 0.0;
    }
}

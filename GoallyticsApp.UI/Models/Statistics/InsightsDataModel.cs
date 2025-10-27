using System.Text.Json.Serialization;

namespace GoallyticsApp.UI.Models.Statistics
{
    public class InsightsDataModel
    {
        [JsonPropertyName("bestMarket")]
        public string BestMarket { get; set; }
        [JsonPropertyName("bestMarketAccuracy")]
        public double BestMarketAccuracy { get; set; }  // yüzde
        [JsonPropertyName("bestLeague")]
        public string BestLeague { get; set; }
        [JsonPropertyName("bestLeagueAccuracy")]
        public double BestLeagueAccuracy { get; set; }  // yüzde
        [JsonPropertyName("avgConfidence")]
        public double AvgConfidence { get; set; }       // yüzde

    }
}

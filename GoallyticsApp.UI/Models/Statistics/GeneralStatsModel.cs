using System.Text.Json.Serialization;

namespace GoallyticsApp.UI.Models.Statistics
{
    public class GeneralStatsModel
    {
        [JsonPropertyName("totalPredictions")]
        public int TotalPredictions { get; set; }
        [JsonPropertyName("correctPredictions")]
        public int CorrectPredictions { get; set; }
        [JsonPropertyName("accuracyRate")]
        public double AccuracyRate { get; set; }
        [JsonPropertyName("totalMatches")]
        public int TotalMatches { get; set; }

    }
}

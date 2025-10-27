using System.Text.Json.Serialization;

namespace GoallyticsApp.UI.Models.Statistics
{
    public class WeeklyPredictionRowModel
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("league")]
        public string League { get; set; }
        [JsonPropertyName("homeTeam")]
        public string HomeTeam { get; set; }
        [JsonPropertyName("awayTeam")]
        public string AwayTeam { get; set; }
        [JsonPropertyName("market")]
        public string Market { get; set; }
        [JsonPropertyName("prediction")]
        public string Prediction { get; set; }
        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
        [JsonPropertyName("result")]
        public string Result { get; set; }
        [JsonPropertyName("isCorrect")]
        public bool? IsCorrect { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}

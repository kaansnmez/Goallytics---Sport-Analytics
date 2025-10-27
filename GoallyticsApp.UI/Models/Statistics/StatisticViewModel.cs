using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace GoallyticsApp.UI.Models.Statistics
{
    public class StatisticViewModel
    {
        [JsonPropertyName("general")]
        public GeneralStatsModel General { get; set; } = new();
        [JsonPropertyName("currentPeriod")]
        public string CurrentPeriod { get; set; }
        [JsonPropertyName("weeklyPrediction")]
        public List<WeeklyPredictionRowModel> WeeklyPredictions { get; set; } = new();
        [JsonPropertyName("charts")]
        public ChartsDataModel Charts { get; set; } = new();
        [JsonPropertyName("markets")]
        public MarketsDataModel Markets { get; set; } = new();
        [JsonPropertyName("insights")]
        public InsightsDataModel Insights { get; set; } = new();
    }
}

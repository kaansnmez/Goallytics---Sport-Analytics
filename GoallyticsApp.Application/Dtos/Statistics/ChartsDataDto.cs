using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class ChartsDataDto
    {
        [JsonPropertyName("marketAccuracy")]
        public MarketAccuracyChartDto MarketAccuracy { get; set; } = new();
        [JsonPropertyName("leagueAccuracy")]
        public LeagueAccuracyChartDto LeagueAccuracy { get; set; } = new();
        [JsonPropertyName("monthlyPerformance")]
        public MonthlyPerformanceChartDto MonthlyPerformance { get; set; } = new();
    }
}

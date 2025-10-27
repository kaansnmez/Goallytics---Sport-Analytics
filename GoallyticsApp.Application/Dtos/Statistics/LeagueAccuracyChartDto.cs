using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class LeagueAccuracyChartDto
    {
        [JsonPropertyName("leagues")]
        public List<string> Leagues { get; set; } = new();
        [JsonPropertyName("accuracies")]
        public List<double> Accuracies { get; set; } = new();
    }
}

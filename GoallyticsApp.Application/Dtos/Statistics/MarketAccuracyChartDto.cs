using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class MarketAccuracyChartDto
    {
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; } = new List<string>();
        [JsonPropertyName("accuracies")]
        public List<double> Accuracies { get; set; } = new();
    }
}

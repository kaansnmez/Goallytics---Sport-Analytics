using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class MonthlyPerformanceChartDto
    {
        [JsonPropertyName("months")]
        public List<string> Months { get; set; } = new();
        [JsonPropertyName("correct")]
        public List<int> Correct { get; set; } = new();
        [JsonPropertyName("incorrect")]
        public List<int> Incorrect { get; set; } = new();
        [JsonPropertyName("accuracy")]
        public List<double> Accuracy { get; set; } = new();
    }
}

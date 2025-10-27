using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ApiGoalsData
    {
        [JsonPropertyName("home")]
        public int? HomeGoal { get; set; }
        [JsonPropertyName("away")]
        public int? AwayGoal { get; set; }
    }
}

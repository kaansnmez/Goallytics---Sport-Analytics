using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ApiGoalsStatisticsList
    {
        [JsonPropertyName("team")]
        public TeamDto? Team { get; set; }
        [JsonPropertyName("statistics")]
        public List<ApiStatisticDto> Stats { get; set; } = new();

        
    }
}

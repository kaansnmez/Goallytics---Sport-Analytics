using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ApiLeagueData
    {
        [JsonPropertyName("id")]
        public int? LeaugeId { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("season")]
        public int? Season { get; set; }
    }
}

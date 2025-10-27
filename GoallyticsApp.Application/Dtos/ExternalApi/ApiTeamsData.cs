using GoallyticsApp.Application.Dtos.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ApiTeamsData
    {
        [JsonPropertyName("home")]
        public TeamDto? HomeTeam { get; set; }
        [JsonPropertyName("away")]
        public TeamDto? AwayTeam { get; set; }
    }
}

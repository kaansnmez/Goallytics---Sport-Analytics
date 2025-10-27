using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class MarketsDataDto
    {
        [JsonPropertyName("market1x2")]
        public MarketCardStatsDto Market1x2 { get; set; }
        [JsonPropertyName("marketOU15")]
        public MarketCardStatsDto OU15 { get; set; }
        [JsonPropertyName("marketOU25")]
        public MarketCardStatsDto OU25 { get; set; }
        [JsonPropertyName("marketOU35")]
        public MarketCardStatsDto OU35 { get; set; }
        [JsonPropertyName("marketBTTS")]
        public MarketCardStatsDto BTTS { get; set; }
        public MarketCardStatsDto Overall { get; set; }
    }
}

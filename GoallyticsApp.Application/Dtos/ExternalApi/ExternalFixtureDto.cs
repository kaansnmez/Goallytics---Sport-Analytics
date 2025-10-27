using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ExternalFixtureDto
    {
        public int FixturesId { get; set; }
        public int LeagueApiId { get; set; }
        public int SeasonApiId { get; set; }
        public string? RoundApiId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime DateUtc { get; set; }
        public string StatusShort { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}

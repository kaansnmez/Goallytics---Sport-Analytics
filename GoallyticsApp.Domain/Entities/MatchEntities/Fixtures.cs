using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.MatchEntities
{
    public class Fixtures : BaseEntity
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

        public Leagues League { get; set; }
        public Season Season { get; set; }
        public Round Round { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public List<Prediction> Predictions { get; set; }

        public List<FixtureStat> FStats { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

    }
}

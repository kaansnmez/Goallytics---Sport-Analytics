using GoallyticsApp.Domain.Entities.MatchEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Match
{
    public class StatisticH2HDto
    {
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public int LeagueApiId { get; set; }
        public int SeasonApiId { get; set; }
        public string? Shots_on_Goal { get; set; }
        public string? Shots_off_Goal { get; set; }
        public string? Total_Shots { get; set; }
        public string? Blocked_Shots { get; set; }
        public string? Shots_insidebox { get; set; }
        public string? Shots_outsidebox { get; set; }
        public string? Fouls { get; set; }
        public string? Corner_Kicks { get; set; }
        public string? Offsides { get; set; }
        public string? Ball_Possession { get; set; }
        public string? Yellow_Cards { get; set; }
        public string? Red_Cards { get; set; }
        public string? Goalkeeper_Saves { get; set; }
        public string? Total_passes { get; set; }
        public string? Passes_accurate { get; set; }
        public string? Passes_Percent { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Match
{
    public class GetAllFixturesDto
    {
        public int FixturesId { get; set; }
        public int? LeagueNameId { get; set; }
        public string? LeagueName { get; set; }
        public int? SeasonApiId { get; set; }
        public string? RoundApiId { get; set; }
        public string? HomeTeamName { get; set; }
        public string? AwayTeamName { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime? DateUtc { get; set; } =DateTime.UtcNow;
        public int? StatusShortId { get; set; }
        public string? StatusShort { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string? HomeIconLink { get; set;}
        public string? AwayIconLink { get;set;}
        public string? Badge { get; set; }
        public List<StatisticFormBadgeDto> FormBadges { get; set; } = new List<StatisticFormBadgeDto>() { new StatisticFormBadgeDto() , new StatisticFormBadgeDto() , new StatisticFormBadgeDto() , new StatisticFormBadgeDto() , new StatisticFormBadgeDto() };

    }
}

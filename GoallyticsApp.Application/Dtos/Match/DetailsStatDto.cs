using GoallyticsApp.Domain.Entities.MatchEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Match
{
    public class DetailsStatDto
    {
        public DateTime? DateUtc { get; set; }
        public RequestTeamDto? HomeTeam { get; set; }
        public RequestTeamDto? AwayTeam { get; set; }
        public StatisticH2HDto FStatHome { get; set; }
        public StatisticH2HDto FStatAway { get; set; }
        public List<GetAllFixturesDto>? HomeForm {  get; set; }
        public List<GetAllFixturesDto>? AwayForm { get; set; }
        public List<H2hMatchDto>? H2HMatch { get; set; }
        public List<IndicatorsDto>? HomeIndicators { get; set; }
        public List<IndicatorsDto>? AwayIndicators { get; set; }
        public GetLeaguesDto? Leauges { get; set; }


    }
}

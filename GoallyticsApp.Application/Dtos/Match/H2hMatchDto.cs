using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Match
{
    public class H2hMatchDto
    {
        public DateTime? DateUtc { get; set; }
        public RequestTeamDto? HomeTeam { get; set; } = new RequestTeamDto();
        public RequestTeamDto? AwayTeam { get; set; } = new RequestTeamDto();
        public GetLeaguesDto? League { get; set; } = new GetLeaguesDto();
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public string? Badge { get; set; } 
    }
}

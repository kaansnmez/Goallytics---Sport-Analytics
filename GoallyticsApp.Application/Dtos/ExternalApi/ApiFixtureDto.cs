using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ApiFixtureDto
    {
        public ApiFixtureData Fixture { get; set; } = new();
        public ApiLeagueData League { get; set; } = new();
        public ApiTeamsData? Teams { get; set; }
        public ApiGoalsData? Goals { get; set; }
        public List<ApiGoalsStatisticsList>? Statistics { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Forecast
{
    public sealed class H2HMatchForcastDto
    {
        public DateTime DateUtc { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }
}

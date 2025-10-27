using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Forecast
{
    public sealed class TeamHistoryForcastDto
    {
        public DateTime DateUtc { get; set; }
        public bool IsHome { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public double? XG { get; set; } 
        public double? XGA { get; set; }

    }
}

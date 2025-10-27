using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class InsightsDataDto
    {
        public string BestMarket { get; set; }
        public double BestMarketAccuracy { get; set; }  // yüzde
        public string BestLeague { get; set; }
        public double BestLeagueAccuracy { get; set; }  // yüzde
        public double AvgConfidence { get; set; }       // yüzde
    }
}

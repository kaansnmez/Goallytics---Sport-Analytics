using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class MarketCardStatsDto
    {
        public int Total { get; set; }
        public int Correct { get; set; }
        public double Accuracy { get; set; }
    }
}

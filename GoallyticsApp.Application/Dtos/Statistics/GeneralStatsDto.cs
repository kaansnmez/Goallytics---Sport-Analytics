using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class GeneralStatsDto
    {
        public int TotalPredictions { get; set; }
        public int CorrectPredictions { get; set; }
        public double AccuracyRate { get; set; }
        public int TotalMatches { get; set; }
    }
}

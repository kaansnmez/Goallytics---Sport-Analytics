using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class StatisticViewDto
    {
        public GeneralStatsDto General { get; set; } = new();
        public string CurrentPeriod { get; set; } = "";
        public List<WeeklyPredictionRowDto> WeeklyPrediction { get; set; } = new();

        public ChartsDataDto Charts { get; set; } = new();
        public MarketsDataDto Markets { get; set; } = new();
        public InsightsDataDto Insights { get; set; } = new();
    }
}

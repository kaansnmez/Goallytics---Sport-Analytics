using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Match
{
    public class IndicatorsDto
    {
        public int HomeTeamId { get; set; }
        public double? AvgGoalMatch { get; set; }
        public double? AvgGoalAway { get; set; }
        public double? AvgShootMatch { get; set; }
        public double? AvgGoalsKale { get; set; }
        public double? AvgBallHandle { get; set; }
        public double? AvgPassSucces { get; set; }
        public double? AvgShootAccurate { get; set; }
        public double? AvgGoalChance { get; set; }
        public double? AvgCorner { get; set; }
        public double? AvgCleanSheet { get; set; }
       }


}

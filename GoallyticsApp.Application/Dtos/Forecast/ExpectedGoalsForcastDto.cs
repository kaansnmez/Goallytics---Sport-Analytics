using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Forecast
{
    public sealed class ExpectedGoalsForcastDto
    {
        public double TotalExpected { get; set; }
        public double HomeExpected { get; set; }
        public double AwayExpected { get; set; }
        public double BaseTotal { get; set; }
        public double XgTotal { get; set; }
        public int H2HCount { get; set; }
    }
}

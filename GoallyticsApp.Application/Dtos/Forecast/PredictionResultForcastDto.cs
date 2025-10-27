using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Forecast
{
    public sealed class PredictionResultForcastDto
    {
        public int FixtureId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public string Market { get; set; }
        public string Prediction { get; set; }
        public double Confidence { get; set; }
        public double ExpectedGoals { get; set; }
        public string Analysis { get; set; }
    }
}

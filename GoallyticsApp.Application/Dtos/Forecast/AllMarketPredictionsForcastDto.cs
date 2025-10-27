using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Forecast
{
    public sealed class AllMarketPredictionsForcastDto
    {
        public int FixtureId { get; set; }
        public DateTime DateUtc { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public PredictionResultForcastDto OU15 { get; set; }
        public PredictionResultForcastDto OU25 { get; set; }
        public PredictionResultForcastDto OU35 { get; set; }
        public PredictionResultForcastDto BTTS { get; set; }
        public PredictionResultForcastDto? Result1x2 { get; set; }
    }
}

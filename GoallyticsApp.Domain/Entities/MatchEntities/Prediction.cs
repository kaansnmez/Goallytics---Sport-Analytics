using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Domain.Entities.MatchEntities
{
    public class Prediction : BaseEntity
    {
        public int FixtureId { get; set; }
        public int LeaugeId { get; set; }
        public DateTime DateUtc { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public string Market { get; set; } = ""; // Over&Under&KG&1x2
        public string Forecast { get; set; } = ""; //over under yes no home draw away
        public double Confidence { get; set; }

        public double ExpectedGoals { get; set; }
        public double HomeExpected { get; set; }
        public double AwayExpected { get; set; }

        public string AnalysisJson { get; set; } = "{}";
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool Result { get; set; }
        public Fixtures Fixtures { get; set; }

        
    }
}

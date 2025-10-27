using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Forecast
{
    public class GeneratePredictionsResultDto
    {
        public int TotalFixtures { get; set; }
        public int PredictionsGenerated { get; set; }
        public int Skipped { get; set; }
        public List<string>? Errors { get; set; }
    }
}

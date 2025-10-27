using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.Statistics
{
    public class ConfidenceAccuracyChartDto
    {
        public List<string> Buckets { get; set; }
        public List<double> Accuracies { get; set; }
    }
}

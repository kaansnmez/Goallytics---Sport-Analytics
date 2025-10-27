namespace GoallyticsApp.UI.Models.Statistics
{
    public class ConfidenceAccuracyChartModel
    {
        public List<string> Buckets { get; set; } = new();
        public List<double> Accuracies { get; set; } = new();
    }
}

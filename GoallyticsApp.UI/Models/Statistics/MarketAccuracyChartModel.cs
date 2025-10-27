namespace GoallyticsApp.UI.Models.Statistics
{
    public class MarketAccuracyChartModel
    {
        public List<string> Labels { get; set; } = new();
        public List<double> Accuracies { get; set; } = new();
    }
}

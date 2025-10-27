namespace GoallyticsApp.UI.Models.Statistics
{
    public class ChartsDataModel
    {
        public MarketAccuracyChartModel MarketAccuracy { get; set; } = new();
        public ConfidenceAccuracyChartModel ConfidenceAccuracy { get; set; } = new();
        public LeagueAccuracyChartModel LeagueAccuracy { get; set; } = new();
        public MonthlyPerformanceChartModel MonthlyPerformance { get; set; } = new();
    }
}

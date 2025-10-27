namespace GoallyticsApp.UI.Models.Statistics
{
    public class MonthlyPerformanceChartModel
    {
        public List<string> Months { get; set; } = new();
        public List<int> Correct { get; set; } = new();
        public List<int> Incorrect { get; set; } = new();

        public List<double> Accuracy { get; set; } = new();

    }
}

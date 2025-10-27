namespace GoallyticsApp.UI.Models.Forcast
{
    public class GetPredictionRequestDtoModel
    {
        public int FixtureId { get; set; } = -1;
        public DateTime? DateUtc { get; set; }
        public int? HomeTeamId { get; set; }
        public int? AwayTeamId { get; set; }
        public PredictionResultForcastDtoModel? OU15 { get; set; } = new();
        public PredictionResultForcastDtoModel? OU25 { get; set; } = new();
        public PredictionResultForcastDtoModel? OU35 { get; set; } = new();
        public PredictionResultForcastDtoModel? BTTS { get; set; } = new();
        public PredictionResultForcastDtoModel? Result1x2 { get; set; } = new();
    }
}

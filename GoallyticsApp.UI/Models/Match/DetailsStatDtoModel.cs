namespace GoallyticsApp.UI.Models.Match
{
    public class DetailsStatDtoModel
    {
        public DetailsStatDtoModel()
        {
            HomeTeam = new();
            AwayTeam = new();
            FStatAway = new();
            FStatHome =new();
            HomeForm = new();
            AwayForm = new();
            H2HMatch = new();
            HomeIndicators = new();
            AwayIndicators = new();
            Leauges = new();
        }
        public int FixtureId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime? DateUtc { get; set; }
        public RequestTeamDtoModel HomeTeam { get; set; } = new RequestTeamDtoModel();
        public RequestTeamDtoModel AwayTeam { get; set; } = new RequestTeamDtoModel();
        public H2HStatisticsDtoModel? FStatHome { get; set; }
        public H2HStatisticsDtoModel? FStatAway { get; set; }
        public List<FixturesListDtoModel>? HomeForm { get; set; }
        public List<FixturesListDtoModel>? AwayForm { get; set; }
        public List<H2hMatchDtoModel>? H2HMatch { get; set; } = new();
        public List<IndicatorsDtoModel>? HomeIndicators { get; set; }
        public List<IndicatorsDtoModel>? AwayIndicators { get; set; }
        public GetLeaguesModel? Leauges { get; set; }
    }
}

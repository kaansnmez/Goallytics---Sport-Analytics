namespace GoallyticsApp.UI.Models.Match
{
    public class H2hMatchDtoModel
    {
        public DateTime? DateUtc { get; set; } = new();
        public RequestTeamDtoModel? HomeTeam { get; set; } = new RequestTeamDtoModel();
        public RequestTeamDtoModel? AwayTeam { get; set; } = new RequestTeamDtoModel();
        public GetLeaguesModel? Leagues { get; set; } = new GetLeaguesModel();
        public int HomeScore { get; set; } = new();
        public int AwayScore { get; set; } = new();
        public string? Badge { get; set; }
    }
}

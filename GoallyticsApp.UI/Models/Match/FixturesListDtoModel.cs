namespace GoallyticsApp.UI.Models.Match
{
    public class FixturesListDtoModel
    {
        public int FixturesId { get; set; }
        public int? LeagueNameId { get; set; }
        public string? LeagueName { get; set; }
        public int? SeasonApiId { get; set; }
        public string? RoundApiId { get; set; }
        public string? HomeTeamName { get; set; }
        public string? AwayTeamName { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime? DateUtc { get; set; } = DateTime.UtcNow;
        public int? StatusShortId { get; set; }
        public string? StatusShort { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string? HomeIconLink { get; set; }
        public string? AwayIconLink { get; set; }
        public string? Badge {  get; set; }
        public List<StatisticFormBadgeDtoModel>? FormBadges { get; set; }
    }
}

namespace GoallyticsApp.UI.Models.Match
{
    public class GetLeaguesModel
    {
        public int Id { get; set; }
        public int LeagueApiId { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Code { get; set; }
    }
}

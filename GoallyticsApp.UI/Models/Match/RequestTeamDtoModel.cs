namespace GoallyticsApp.UI.Models.Match
{
    public class RequestTeamDtoModel
    {
        public int? TeamApiId { get; set; }
        public string? Name { get; set; }  
        public string? Code { get; set; }
        public bool? National { get; set; }
    }
}

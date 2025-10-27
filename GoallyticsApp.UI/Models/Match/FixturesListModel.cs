using GoallyticsApp.UI.Models.Forcast;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoallyticsApp.UI.Models.Match
{
    public class FixturesListModel
    {
        public DateTime DateUtc { get; set; } = DateTime.UtcNow;
        public int? FixtureId { get; set; }
        public int? StatusShortId { get; set; }
        public int? LeagueNameId { get; set; }
        public SelectList? Status { get; set; }
        public SelectList? League { get; set; }
        public List<FixturesListDtoModel>? Fixtures { get; set; }
        public DetailsStatDtoModel? DetailStats { get; set; }
        public GetPredictionRequestDtoModel? Forcast { get; set; } = new();
    }
}

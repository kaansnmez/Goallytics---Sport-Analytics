
using GoallyticsApp.UI.Models.Forcast;
using GoallyticsApp.UI.Models.Match;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GoallyticsApp.UI.Controllers
{
    public class FixturesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FixturesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44390/api/Match/GetLeauges");
            if (response.IsSuccessStatusCode)
            {
                var stat = new[]
                {
                    new{Text = "Planned",value="1"},
                    new{Text = "Finished",value="2"}
                };
                var rawJson = await response.Content.ReadAsStringAsync();
                var leagueList = JsonSerializer.Deserialize<List<GetLeaguesModel>>(rawJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return View(new FixturesListModel
                {
                    League = new SelectList(leagueList, "LeagueApiId", "Name"),
                    Status = new SelectList(stat,"value","Text")
                    
                });
            }
            return View(new FixturesListModel());
            
        }
        [HttpPost]
        public async Task<IActionResult> Index(FixturesListModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44390/api/Match/GetLeauges");
            if (response.IsSuccessStatusCode)
            {
                var stat = new[]
                {
                    new{Text = "Planned",value="1"},
                    new{Text = "Finished",value="2"}
                };
                var rawJson = await response.Content.ReadAsStringAsync();
                var leagueList = JsonSerializer.Deserialize<List<GetLeaguesModel>>(rawJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                model.League = new SelectList(leagueList, "LeagueApiId", "Name", model.LeagueNameId);
                model.Status = new SelectList(stat, "value", "Text", model.StatusShortId);
                
            }
            if (ModelState.IsValid)
            {
                var fixturesDto = new FixturesListModel
                {
                    LeagueNameId=model.LeagueNameId,
                    StatusShortId=model.StatusShortId,
                    DateUtc=model.DateUtc

                };
                var content = new StringContent(JsonSerializer.Serialize(fixturesDto), Encoding.UTF8, "application/json");
                var responsePost = await client.PostAsync("https://localhost:44390/api/Match/ListFixtures", content);
                if (responsePost.IsSuccessStatusCode)
                {
                    var rawJson = await responsePost.Content.ReadAsStringAsync();
                    var fixtureList = JsonSerializer.Deserialize<List<FixturesListDtoModel>>(rawJson, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    model.Fixtures = fixtureList; // fixture model ataması
                    return View(model);
                }
                
                
            }
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetDetails(FixturesListModel fixtureModel,int HomeTeamId, int AwayTeamId, int FixtureId)
        {
            var client = _httpClientFactory.CreateClient();
            var model = new DetailsStatDtoModel
            {
                FixtureId = FixtureId,
                HomeTeamId = HomeTeamId,
                AwayTeamId = AwayTeamId
            };
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var responsePost = await client.PostAsync("https://localhost:44390/api/Match/GetStatsById", content);
            if (responsePost.IsSuccessStatusCode)
            {
                var rawJson = await responsePost.Content.ReadAsStringAsync();
                DetailsStatDtoModel detailsStat = new();
                detailsStat = JsonSerializer.Deserialize<DetailsStatDtoModel>(rawJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                detailsStat.HomeTeam ??= new ();
                detailsStat.AwayTeam ??= new ();
                detailsStat.FStatAway ??= new ();
                detailsStat.FStatHome ??= new ();
                detailsStat.HomeForm ??= new ();
                detailsStat.AwayForm ??= new ();
                detailsStat.H2HMatch ??= new ();
                detailsStat.HomeIndicators ??= new();
                detailsStat.AwayIndicators ??= new();
                detailsStat.Leauges ??= new();

                return PartialView("_DetailsStat",detailsStat);
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> GetPredictions(int fixtureId)
        {
            var client = _httpClientFactory.CreateClient();
            var model = new GetPredictionRequestDtoModel() { FixtureId = fixtureId };
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var responsePost = await client.PostAsync("https://localhost:44390/api/Match/GetPredictionsById", content);
            if (responsePost.IsSuccessStatusCode)
            {
                GetPredictionRequestDtoModel forcastModel = new();
                var rawJson = await responsePost.Content.ReadAsStringAsync();
                var preds = JsonSerializer.Deserialize<GetPredictionRequestDtoModel>(rawJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return PartialView("_Forcasting", preds);

            }
            return View(fixtureId);
        }
    }
}

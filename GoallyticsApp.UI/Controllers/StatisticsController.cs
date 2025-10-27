using GoallyticsApp.UI.Models.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace GoallyticsApp.UI.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StatisticsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string period = "current")
        {
            var model = new StatisticViewModel();
            var token = User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
            if (token!=null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var queryString = $"?period={Uri.EscapeDataString(period)}";
                var response = await client.GetAsync($"https://localhost:44390/api/Statistic/GetStatisticsByPeriod{queryString}");
                if (response.IsSuccessStatusCode)
                {
                    var rawJson = await response.Content.ReadAsStringAsync();
                    model = System.Text.Json.JsonSerializer.Deserialize<StatisticViewModel>(rawJson, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
                    });

                }

                return View(model);
            }
            return View();
        }
    }
}

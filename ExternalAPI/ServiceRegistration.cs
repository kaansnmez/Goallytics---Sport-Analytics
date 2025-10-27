using ExternalAPI.ExternalAPI;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Persistance.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
namespace ExternalAPI
{
    public static class ServiceRegistration
    {
        public static void AddExternalAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExternalApiClient, ExternalApiClient>();
            services.Configure<FootballApiOptions>(options =>
            {
                configuration.GetSection("FootballApi");
                });
            services.AddHttpClient<IExternalApiClient, ExternalApiClient>((sp, http) =>
            {
                var opt = sp.GetRequiredService<IConfiguration>();

                http.BaseAddress = new Uri(opt["FootballApi:BaseUrl"]);
                http.Timeout = TimeSpan.FromSeconds(int.Parse(opt["FootballApi:TimeoutSeconds"]));
                http.DefaultRequestHeaders.Add("x-apisports-key", opt["FootballApi:ApiKey"]);
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
                .AddPolicyHandler(PollyPolicies.GetRetryWithJitter())
                .AddPolicyHandler(PollyPolicies.CircuitBraker())
                .AddPolicyHandler(PollyPolicies.Timeout());
            services.AddHostedService<FixtureSyncHostedService>();
        }
    }
}

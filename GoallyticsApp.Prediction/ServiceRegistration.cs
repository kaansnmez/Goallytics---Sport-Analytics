using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Prediction.Repositories;
using GoallyticsApp.Prediction.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoallyticsApp.Prediction
{
    public static class ServiceRegistration
    {
        public static void AddPredictionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPredictionCalculator, PredictionCalculator>();
            services.AddHostedService<PredictionHostedService>();

        }
    }
}
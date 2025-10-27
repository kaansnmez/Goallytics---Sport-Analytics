using GoallyticsApp.Application.Features.CQRS.Commands.Forcast;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Prediction.Services
{
    public sealed class PredictionHostedService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<PredictionHostedService> _logger;
        private readonly TimeSpan _interval;
        public PredictionHostedService(ILogger<PredictionHostedService> logger, IServiceProvider sp,IConfiguration cfg)
        {
            _logger = logger;
            _sp = sp;
            var intervalHours = cfg.GetValue<int?>("PredictionSync:IntervalHours") ?? 12;
           _interval = TimeSpan.FromHours(intervalHours);

            _logger.LogInformation("PredictionHostedService initialized with interval : {interval} hours", intervalHours);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PredictionHostedService waiting 30 seconds before first run...");
            await Task.Delay(TimeSpan.FromSeconds(30),stoppingToken);
            
            using var timer = new PeriodicTimer(_interval);

            _logger.LogInformation("PredictionHostedService started. Running every {Interval}", _interval);

            do
            {
                try
                {
                    _logger.LogInformation("Starting prediction generation...");
                    using var scope = _sp.CreateScope();
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var boolDate = DateTime.TryParseExact("2016-08-26","yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime startDate );
                    var result = await _mediator.Send(
                        new GeneratePredictionsCommand()
                        {
                            StartDate = startDate,
                            EndDate = DateTime.UtcNow.AddDays(7),
                            LeagueId = null,
                            MinConfidenceThreshold = 61
                        },stoppingToken);

                    _logger.LogInformation(
                    "Prediction generation complete: {Generated}/{Total} predictions, {Skipped} skipped, {Errors} errors",
                    result.PredictionsGenerated,
                    result.TotalFixtures,
                    result.Skipped,
                    result.Errors.Count);

                    if (result.Errors.Any())
                    {
                        foreach (var error in result.Errors.Take(5))
                        {
                            _logger.LogWarning("Prediction error: {Error}", error);
                        }
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("PredictionHostedService is stopping...");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Prediction generation failed");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
            _logger.LogInformation("PredictionHostedService stopped");
        }
    }
}

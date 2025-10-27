using GoallyticsApp.Application.Features.CQRS.Commands.ExternalApi;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalAPI.ExternalAPI
{
    public sealed class FixtureSyncOptions
    {
        public int IntervalMinutes { get; set; } = 15;
    }
    public class FixtureSyncHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FixtureSyncHostedService> _logger;
        private readonly FixtureSyncOptions _options;

        public FixtureSyncHostedService(IOptions<FixtureSyncOptions> options, IServiceProvider serviceProvider, ILogger<FixtureSyncHostedService> logger , IConfiguration cfg)
        {
            
            _serviceProvider = serviceProvider;
            _logger = logger;
            var minutes = cfg.GetValue<int?>("FixtureSync : IntervalMinutes") ?? 15;
            _options = options.Value;
        }
        
        protected override async Task ExecuteAsync (CancellationToken stoppingToken)
        {
            await Task.Delay (TimeSpan.FromSeconds(15),stoppingToken);
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(_options.IntervalMinutes));
            do
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    
                    await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(250,1500)),stoppingToken);
                    await mediator.Send(new ExternalUpdateFixtureByIdCommand());
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) 
                {
                    break;
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "Fixture sync failed.");
                }

            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}

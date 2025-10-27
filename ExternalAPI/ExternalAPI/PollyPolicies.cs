using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExternalAPI.ExternalAPI
{
    public static class PollyPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryWithJitter() =>
            HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => r.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(
                retryCount: 4,
                sleepDurationProvider: (i, ctx) =>
                {
                    if (ctx.TryGetValue("RetryAfter", out var v) && v is TimeSpan ra)
                        return ra;
                    var backoff = TimeSpan.FromSeconds(Math.Pow(2, i));
                    var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(100, 750));
                    return backoff + jitter;
                },
                onRetryAsync: async (outcome, ts, tryNo, ctx) =>
                {
                    if (outcome.Result?.StatusCode == (HttpStatusCode)429)
                    {
                        var retryAfter = outcome.Result.Headers.RetryAfter?.Delta ?? TimeSpan.Zero;
                        ctx["RetryAfter"] = retryAfter;
                    }
                    await Task.CompletedTask;
                });
        public static IAsyncPolicy<HttpResponseMessage> CircuitBraker()
            => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking:4,durationOfBreak:TimeSpan.FromSeconds(30));
       public static IAsyncPolicy<HttpResponseMessage> Timeout()
            => Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));
    }
}

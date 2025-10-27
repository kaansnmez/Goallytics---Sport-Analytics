using AutoMapper;
using GoallyticsApp.Application.Dtos.ExternalApi;
using GoallyticsApp.Application.Dtos.Match;
using GoallyticsApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExternalAPI.ExternalAPI
{
    public class ExternalApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiClient> _logger;

        public ExternalApiClient(HttpClient httpClient, IConfiguration configuration, ILogger<ExternalApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<ApiFixtureResponseDto> GetFixtureByIdAsync(int fixtureId, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, $"/fixtures?id={fixtureId}");
            using var resp = await _httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            if (resp.StatusCode == (HttpStatusCode)429)
            {
                var retryAfter = resp.Headers.RetryAfter?.Delta;
                _logger.LogWarning("429 reveived for FixtureId {FixtureId}, retry-after {RetryAfter}", fixtureId, retryAfter);
                return null; //Polly Retry again
            }
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                _logger.LogError("Fixture get failed : {Status} - {Body}", resp.StatusCode, body);
                return null;
            }
            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            var root = await JsonSerializer.DeserializeAsync<ApiFixtureResponseDto>(stream, SerializerOptions,ct);
            if (root?.Response is null)
            {
                _logger.LogWarning("Fixture {FixtureId} not found or not accessible", fixtureId);
                return null;
            }
            //var apiItem = root.Response;
            return root;

        }
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    }
}

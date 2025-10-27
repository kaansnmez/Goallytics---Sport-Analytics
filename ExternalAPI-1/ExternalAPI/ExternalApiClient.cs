using AutoMapper;
using GoallyticsApp.Application.Dtos.ExternalApi;
using GoallyticsApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalAPI.ExternalAPI
{
    public class ExternalApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;

        public ExternalApiClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["ExternalApi:BaseUrl"]);
        }
        public async Task<ExternalFixtureDto> GetFixtureByIdAsync (int fixtureId)
        {
            return await _httpClient.GetFromJsonAsync<ExternalFixtureDto>($"fixtures/{fixtureId}");
        }
    }
}

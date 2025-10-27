using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ApiFixtureResponseDto
    {
        [JsonPropertyName("get")]
        public string Get { get; set; } = "";

        public object? Parameters { get; set; }

        public List<string> Errors { get; set; } = new();

        public int Results { get; set; }

        public object? Paging { get; set; }

        public List<ApiFixtureDto> Response { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos.ExternalApi
{
    public class ApiStatisticDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; } = "";
        [JsonPropertyName("value")]
        public JsonElement? Value { get; set; } = new();

        //Helper
        public int GetIntValue() => Value.Value.ValueKind == JsonValueKind.Number ? Value.Value.GetInt32() : 0;
        public string GetStringValue() => Value.Value.ValueKind == JsonValueKind.String ? Value.Value.GetString() ?? "":"";
        public double GetDoubleValue() => Value.Value.ValueKind == JsonValueKind.Number ? Value.Value.GetDouble() : 0.0;
        public string GetFormattedValue()
        {
            return Value.Value.ValueKind switch
            {
                JsonValueKind.Number => Value.Value.GetInt32().ToString(),
                JsonValueKind.String => Value.Value.GetString() ?? "",
                _=> ""
            };
        }
    }
}

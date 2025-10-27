using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalAPI.ExternalAPI
{
    public sealed class FootballApiOptions
    {
        public string BaseUrl { get; init; } = "";
        public string ApiKey { get; init; } = "";
        public int TimeoutSeconds { get; init; } = 30;
    }
}

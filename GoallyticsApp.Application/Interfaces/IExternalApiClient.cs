using GoallyticsApp.Application.Dtos.ExternalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Interfaces
{
    public interface IExternalApiClient
    {
        Task<ApiFixtureResponseDto> GetFixtureByIdAsync(int fixtureId, CancellationToken ct = default);
    }
}

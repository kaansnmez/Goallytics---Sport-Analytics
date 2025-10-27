using GoallyticsApp.Application.Dtos.Forecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Interfaces
{
    public interface IPredictionCalculator
    {
        Task<AllMarketPredictionsForcastDto?> CalculateAllMarketsAsync(
            int fixtureId,
            int homeTeamId,
            int awayTeamId,
            DateTime fixtureDate,
            CancellationToken ct = default);
    }
}

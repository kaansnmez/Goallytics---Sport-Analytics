using AutoMapper;
using GoallyticsApp.Application.Features.CQRS.Commands.ExternalApi;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.ExternalApi
{
    public class ExternalUpdateFixtureByIdCommandHandler : IRequestHandler<ExternalUpdateFixtureByIdCommand>
    {
        private readonly IExternalApiClient _externalApiClient;
        private readonly IUow _uow;
        private readonly ILogger<ExternalUpdateFixtureByIdCommandHandler> _logger;
        private readonly IMapper _mapper;
        public ExternalUpdateFixtureByIdCommandHandler(IExternalApiClient externalApiClient, IUow uow, ILogger<ExternalUpdateFixtureByIdCommandHandler> logger, IMapper mapper)
        {
            _externalApiClient = externalApiClient;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<Unit> Handle(ExternalUpdateFixtureByIdCommand request, CancellationToken cancellationToken)
        {
            Dictionary<string, string> FixtureStatDbMap = new()
                    {
                        { "Shots on Goal","Shots_on_Goal"},
                        { "Shots off Goal","Shots_off_Goal"},
                        {"Total Shots","Total_Shots" },
                        {"Blocked Shots","Blocked_Shots"},
                        {"Shots insidebox","Shots_insidebox"},
                        {"Shots outsidebox","Shots_outsidebox"},
                        {"Fouls","Fouls"},
                        {"Corner Kicks","Corner_Kicks"},
                        {"Offsides","Offsides"},
                        {"Ball Possession","Ball_Possession"},
                        {"Yellow Cards","Yellow_Cards"},
                        {"Red Cards","Red_Cards"},
                        {"Goalkeeper Saves","Goalkeeper_Saves"},
                        {"Total passes","Total_passes"},
                        {"Passes accurate","Passes_accurate"},
                        {"Passes %","Passes_Percent"}
                    };
            var today = DateTime.Now.AddHours(-3);
            var check = await _uow.GetRepository<Fixtures>().QueryToListAsync(f => f.StatusShort == "FT" && (f.HomeScore==null || f.AwayScore==null),null,0,default,f => f.FStats);
            if (check.Count > 0)
            {
                foreach (var item in check)
                {
                    item.StatusShort = "NS";
                    await _uow.GetRepository<Fixtures>().UpdateAsync(item);
                }
            }
            var ids = await _uow.GetRepository<Fixtures>().QueryToListAsync(f => f.StatusShort == "NS" && f.DateUtc < today, null, 0,default,f=>f.FStats);
            
            foreach (var id in ids)
            {
                var dto = await _externalApiClient.GetFixtureByIdAsync(id.FixturesId, cancellationToken);
                var fstats = await _uow.GetRepository<FixtureStat>().QueryToListAsync(f => f.FixtureId == id.FixturesId, null, 0);
                if (dto is null)
                    continue;
                foreach (var item in dto.Response)
                {
                    if ((item.Fixture.Date!=id.DateUtc || item.Fixture.Date.Day!=id.Day))
                    {
                        id.DateUtc = item.Fixture.Date;
                        id.Day = item.Fixture.Date.Day;
                        id.Month = item.Fixture.Date.Month;
                        id.Year = item.Fixture.Date.Year;
                        await _uow.GetRepository<Fixtures>().UpdateAsync(id);
                        break;
                    }
                    id.HomeScore = item.Goals.HomeGoal;
                    id.AwayScore = item.Goals.AwayGoal;
                    
                    if (item.Statistics.Count>0)
                    {
                        for (int i = 0; i < item.Statistics.Count; i++)
                        {
                            if (id.FStats.Count < 2)
                            {
                                id.FStats.Add(new FixtureStat());
                                id.FStats[i].FixtureId = item.Fixture.Id;
                                id.FStats[i].TeamId = item.Teams.HomeTeam.Id;
                                id.FStats[i].TeamName = item.Teams.HomeTeam.Name;
                                if (i == 1)
                                {
                                    id.FStats[i].TeamId = item.Teams.AwayTeam.Id;
                                    id.FStats[i].TeamName = item.Teams.AwayTeam.Name;
                                }
                                id.FStats[i].DateUtc = item.Fixture.Date;
                                id.FStats[i].HomeTeamId = item.Teams.HomeTeam.Id;
                                id.FStats[i].AwayTeamId = item.Teams.AwayTeam.Id;

                                id.FStats[i].LeagueApiId = item.League.LeaugeId ?? -1;
                                id.FStats[i].SeasonApiId = item.League.Season ?? -1;
                            }
                            id.FStats[i].HomeScore = item.Goals.HomeGoal;
                            id.FStats[i].AwayScore = item.Goals.AwayGoal;

                            foreach (var statsitem in item.Statistics[i].Stats)
                            {
                                string? apiValue = statsitem.Type;
                                if (FixtureStatDbMap.TryGetValue(apiValue, out string? dbValue))
                                {
                                    Console.WriteLine($"API Key: {(string?)statsitem.Type} eşleşti -> DB Key: {dbValue}");
                                    switch (dbValue)
                                    {
                                        case "Shots_on_Goal":
                                            id.FStats[i].Shots_on_Goal = statsitem.Value.ToString();
                                            break;
                                        case "Shots_off_Goal":
                                            id.FStats[i].Shots_off_Goal = statsitem.Value.ToString();
                                            break;
                                        case "Total_Shots":
                                            id.FStats[i].Total_Shots = statsitem.Value.ToString();
                                            break;
                                        case "Blocked_Shots":
                                            id.FStats[i].Blocked_Shots = statsitem.Value.ToString();
                                            break;
                                        case "Shots_insidebox":
                                            id.FStats[i].Shots_insidebox = statsitem.Value.ToString();
                                            break;
                                        case "Shots_outsidebox":
                                            id.FStats[i].Shots_outsidebox = statsitem.Value.ToString();
                                            break;
                                        case "Fouls":
                                            id.FStats[i].Fouls = statsitem.Value.ToString();
                                            break;
                                        case "Corner_Kicks":
                                            id.FStats[i].Corner_Kicks = statsitem.Value.ToString();
                                            break;
                                        case "Offsides":
                                            id.FStats[i].Offsides = statsitem.Value.ToString();
                                            break;
                                        case "Ball_Possession":
                                            id.FStats[i].Ball_Possession = statsitem.Value.ToString();
                                            break;
                                        case "Yellow_Cards":
                                            id.FStats[i].Yellow_Cards = statsitem.Value.ToString();
                                            break;
                                        case "Red_Cards":
                                            id.FStats[i].Red_Cards = statsitem.Value.ToString();
                                            break;
                                        case "Goalkeeper_Saves":
                                            id.FStats[i].Goalkeeper_Saves = statsitem.Value.ToString();
                                            break;
                                        case "Total_passes":
                                            id.FStats[i].Total_passes = statsitem.Value.ToString();
                                            break;
                                        case "Passes_accurate":
                                            id.FStats[i].Passes_accurate = statsitem.Value.ToString();
                                            break;
                                        case "Passes_Percent":
                                            id.FStats[i].Passes_Percent = statsitem.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }

                                }
                                id.StatusShort = "FT";
                            }

                        }

                    }
                    
                    
                }

                
                await _uow.GetRepository<Fixtures>().UpdateAsync(id);
                _logger.LogInformation("Fixture sync completed.Id = {fixtureId} Count = {count}",id.FixturesId,ids.Count);
                
            }
            return Unit.Value;
        }
    }
}

using AutoMapper;
using GoallyticsApp.Application.Dtos.Match;
using GoallyticsApp.Application.Enums;
using GoallyticsApp.Application.Features.CQRS.Queries.Match;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Match
{
    public class GetDetailsByTeamIdQueryRequestHandler : IRequestHandler<GetDetailsByTeamIdQueryRequest, DetailsStatDto>
    {
        private readonly IUow _uow;
        private readonly IRepository<FixtureStat> _stats;
        private readonly IMapper _mapper;

        public GetDetailsByTeamIdQueryRequestHandler(IRepository<FixtureStat> stats, IUow uow, IMapper mapper)
        {
            _stats = stats;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<DetailsStatDto> Handle(GetDetailsByTeamIdQueryRequest request, CancellationToken cancellationToken)
        {
            
           
            var raw_h2hMatch = await _uow.GetRepository<Fixtures>().QueryToListAsync(
                f => ((f.HomeTeamId == request.HomeTeamId &&
                f.AwayTeamId == request.AwayTeamId) || (f.AwayTeamId == request.AwayTeamId &&
                f.HomeTeamId == request.HomeTeamId)) && f.StatusShort == "FT",f=>f.DateUtc,7,default,f => f.HomeTeam, f => f.AwayTeam, f => f.League);
            
            var raw_form_home = await _uow.GetRepository<Fixtures>().QueryToListAsync(
                f => f.HomeTeamId == request.HomeTeamId && f.StatusShort == "FT", f=>f.DateUtc, 5,default,f => f.HomeTeam, f => f.AwayTeam);
            var raw_form_away = await _uow.GetRepository<Fixtures>().QueryToListAsync(
                f => f.AwayTeamId == request.AwayTeamId && f.StatusShort == "FT", f=>f.DateUtc, 5, default, f => f.HomeTeam, f => f.AwayTeam);
            //------Indicators-----------
            var overallStatsHome_n10 = await _uow.GetRepository<FixtureStat>().QueryToListAsync(
               f => (f.TeamId==request.HomeTeamId && f.HomeTeamId == request.HomeTeamId) && f.Fixtures.StatusShort=="FT" , f=>f.DateUtc, 10,default, f => f.Fixtures);
            var overallStatsAway_n10 = await _uow.GetRepository<FixtureStat>().QueryToListAsync(
               f => (f.TeamId==request.AwayTeamId && f.AwayTeamId == request.AwayTeamId) && f.Fixtures.StatusShort == "FT", f => f.DateUtc, 10,default, f => f.Fixtures);
            

            //Calculate Indicators
            long ParseInt(string? s)
            {
                if (string.IsNullOrWhiteSpace(s))
                    return 0;
                s = s.Replace("%", "").Trim();
                return int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;
            }
            double ParseDouble(string? s)
            {
                if (string.IsNullOrEmpty(s))
                    return 0;
                s = s.Replace("%", "").Trim();
                return double.TryParse(s,out var d) ? d : 0;
            }
            var homeStatIndicators = overallStatsHome_n10.GroupBy(f => f.HomeTeamId).Select(group =>
            new IndicatorsDto
            {
                HomeTeamId = group.Key,
                //Goller
                AvgGoalMatch = group.Average(fs => (double?)fs.HomeScore),
                AvgGoalAway = group.Average(fs => (double?)fs.AwayScore),
                //Sutlar
                AvgShootMatch = group.Average(fs => (double)ParseInt(fs.Total_Shots)),
                //Kalede görülen şut
                AvgGoalsKale = group.Average(fs=>
                {
                    var total = ParseInt(fs.Total_Shots);
                    var off = ParseInt(fs.Shots_off_Goal);
                    var block = ParseInt(fs.Blocked_Shots);
                    return (double)(total-off-block);
                }),
                //top hakimiyeti
                AvgBallHandle = group.Average(fs => (double)ParseInt(fs.Ball_Possession)),
                AvgPassSucces = group.Average(fs => (double)ParseInt(fs.Passes_Percent)),
                
                //Ceza Sahası içi şutlar baz alan metrikler
                AvgShootAccurate = group.Average(fs => ParseInt(fs.Shots_insidebox)),
                AvgGoalChance = Math.Round((group.Average(fs => (double)ParseInt(fs.Shots_insidebox)) * 0.35),2),
                //Korner
                AvgCorner = group.Average(fs=> (double)ParseInt(fs.Corner_Kicks)),
                //Clean sheet
                AvgCleanSheet=group.Average(fs=>fs.AwayScore==0?1.0:0.0)

            }).ToList();
            var awayStatIndicators = overallStatsAway_n10.GroupBy(f => f.AwayTeamId).Select(group =>
            new IndicatorsDto
            {
                HomeTeamId = group.Key,
                //Goller
                AvgGoalMatch = group.Average(fs => (double?)fs.HomeScore),
                AvgGoalAway = group.Average(fs => (double?)fs.AwayScore),
                //Sutlar
                AvgShootMatch = group.Average(fs => (double)ParseInt(fs.Total_Shots)),
                //Kalede görülen şut
                AvgGoalsKale = group.Average(fs =>
                {
                    var total = ParseInt(fs.Total_Shots);
                    var off = ParseInt(fs.Shots_off_Goal);
                    var block = ParseInt(fs.Blocked_Shots);
                    return (double)(total - off - block);
                }),
                //top hakimiyeti
                AvgBallHandle = group.Average(fs => (double)ParseInt(fs.Ball_Possession)),
                AvgPassSucces = group.Average(fs => (double)ParseInt(fs.Passes_Percent)),

                //Ceza Sahası içi şutlar baz alan metrikler
                AvgShootAccurate = group.Average(fs => ParseInt(fs.Shots_insidebox)),
                AvgGoalChance = Math.Round((group.Average(fs => (double)ParseInt(fs.Shots_insidebox)) * 0.35), 2),
                //Korner
                AvgCorner = group.Average(fs => (double)ParseInt(fs.Corner_Kicks)),
                //Clean sheet
                AvgCleanSheet = group.Average(fs => fs.HomeScore == 0 ? 1.0 : 0.0)

            }).ToList();

        
            //----------------------------------------
            var returnDto = new DetailsStatDto();
            var Leauge = new GetLeaguesDto();
            if (raw_h2hMatch.Count() == 0)
            {
                returnDto = new DetailsStatDto();
            }
            else
                foreach (var ent in raw_h2hMatch)
                {
                    Leauge = _mapper.Map<GetLeaguesDto>(ent.League);
                    returnDto = new DetailsStatDto
                    {
                        DateUtc = ent.DateUtc,
                        HomeTeam = (_mapper.Map<RequestTeamDto>(ent.HomeTeam)) ?? new RequestTeamDto(),
                        AwayTeam = (_mapper.Map<RequestTeamDto>(ent.AwayTeam)) ?? new RequestTeamDto(),
                        Leauges = Leauge
                    };
                }
            var h2hMatchdto = _mapper.Map<List<H2hMatchDto>>(raw_h2hMatch);
            var formHomeDto = _mapper.Map<List<GetAllFixturesDto>>(raw_form_home);
            var formAwayDto = _mapper.Map<List<GetAllFixturesDto>>(raw_form_away);
            if (formHomeDto.Count > 0)
            {
                foreach (var item in formHomeDto)
                {
                    if (item.HomeScore > item.AwayScore)
                        item.Badge = "G";
                    if (item.AwayScore > item.HomeScore)
                        item.Badge = "M";
                    if (item.AwayScore == item.HomeScore)
                        item.Badge = "B";

                }
            }
            if (formAwayDto.Count > 0)
            {
                foreach (var item in formAwayDto)
                {
                    if (item.HomeScore > item.AwayScore)
                        item.Badge = "G";
                    if (item.AwayScore > item.HomeScore)
                        item.Badge = "M";
                    if (item.AwayScore == item.HomeScore)
                        item.Badge = "B";
                }
            }
            
            returnDto.H2HMatch = h2hMatchdto;
            returnDto.HomeForm = formHomeDto;
            returnDto.AwayForm = formAwayDto;
            returnDto.AwayIndicators = awayStatIndicators;
            returnDto.HomeIndicators = homeStatIndicators;
            return returnDto;
        }
    }
}

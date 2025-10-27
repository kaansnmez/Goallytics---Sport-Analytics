using AutoMapper;
using AutoMapper.Features;
using GoallyticsApp.Application.Dtos.Match;
using GoallyticsApp.Application.Enums;
using GoallyticsApp.Application.Features.CQRS.Queries.Match;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities.MatchEntities;
using MediatR;
using Microsoft.Win32;

//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Features.CQRS.Handlers.Match
{
    public class GetAllFixturesQueryRequestHandler : IRequestHandler<GetAllFixturesQueryRequest, List<GetAllFixturesDto>>
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IRepository<Fixtures> _fixtures;
        public GetAllFixturesQueryRequestHandler(IUow uow, IRepository<Fixtures> fixtures, IMapper mapper)
        {
            _uow = uow;
            _fixtures = fixtures;
            _mapper = mapper;
        }

        public async Task<List<GetAllFixturesDto>> Handle(GetAllFixturesQueryRequest request, CancellationToken cancellationToken)
        {
            var Year = (int)request.DateUtc.Year;
            var Day = (int)request.DateUtc.Day;
            var Month = (int)request.DateUtc.Month;
            var fixtures = new List<Fixtures>();
            var h2hDto = new StatisticH2HDto();

            if (request.LeagueNameId != null && request.LeagueNameId!=0)
            {
                if (request.StatusShortId != null && request.StatusShortId != (int)StatusShortType.All)
                {
                    if (request.StatusShortId != (int)StatusShortType.Planned)
                    {
                         fixtures = await
                    _uow.GetRepository<Fixtures>().QueryToListAsync(
                        f =>
                        f.StatusShort == "NS" && 
                        f.League.LeagueApiId == request.LeagueNameId &&
                        f.Day == Day &&
                        f.Month == Month &&
                        f.Year == Year,
                        null, 0, cancellationToken,
                        f => f.HomeTeam, f => f.AwayTeam, f => f.League);
                       
                    }
                    else
                    {
                         fixtures = await
                    _uow.GetRepository<Fixtures>().QueryToListAsync(
                        f => 
                        f.StatusShort == "FT" && 
                        f.League.LeagueApiId == request.LeagueNameId &&
                        f.Day == Day &&
                        f.Month == Month &&
                        f.Year == Year,
                        null, 0, cancellationToken,
                        f => f.HomeTeam, f => f.AwayTeam, f => f.League);
                    }
                }
                else
                {
                    if (request.StatusShortId != (int)StatusShortType.Planned)
                    {
                         fixtures = await
                    _uow.GetRepository<Fixtures>().QueryToListAsync(
                        f => 
                        f.League.LeagueApiId == request.LeagueNameId &&
                        f.Day == Day &&
                        f.Month == Month &&
                        f.Year == Year,
                        null, 0,
                        cancellationToken,
                        f => f.HomeTeam, f => f.AwayTeam, f => f.League);
                    }
                    else
                    {
                         fixtures = await
                    _uow.GetRepository<Fixtures>().QueryToListAsync(
                        f =>  
                        f.League.LeagueApiId == request.LeagueNameId &&
                        f.Day == Day &&
                        f.Month == Month &&
                        f.Year == Year,
                        null, 0,
                        cancellationToken,
                        f => f.HomeTeam, f => f.AwayTeam, f => f.League);
                    }

                }
            }
            else
            {
                if (request.StatusShortId != null && request.StatusShortId != (int)StatusShortType.All)
                {
                    if (request.StatusShortId == (int)StatusShortType.Planned)
                    {
                         fixtures = await
                    _uow.GetRepository<Fixtures>().QueryToListAsync(
                        f => 
                        f.StatusShort == "NS" &&
                        f.Day == Day &&
                        f.Month == Month &&
                        f.Year == Year,
                        null, 0,
                        cancellationToken,
                        f => f.HomeTeam,f => f.AwayTeam, f => f.League);
                    }
                    else
                    {
                         fixtures = await
                    _uow.GetRepository<Fixtures>().QueryToListAsync(
                        f => 
                        f.StatusShort == "FT" &&
                        f.Day == Day &&
                        f.Month == Month &&
                        f.Year == Year,
                        null, 0,
                        cancellationToken,
                        f => f.HomeTeam, f => f.AwayTeam, f => f.League);
                    }
                }
                else
                {
                    if (request.StatusShortId != (int)StatusShortType.Planned)
                    {
                         fixtures = await
                    _uow.GetRepository<Fixtures>().QueryToListAsync(f=>
                        f.Day == Day &&
                        f.Month == Month &&
                        f.Year == Year,
                        null, 0,
                        cancellationToken,
                        f => f.HomeTeam, f => f.AwayTeam, f => f.League);
                    }
                    
                }
            }
            var rev= _mapper.Map<List<GetAllFixturesDto>>(fixtures);
            if (rev.Count!=0)
            {
                for (int i = 0; i < rev.Count; i++)
                {
                    rev[i].HomeIconLink = fixtures[i].HomeTeam.Logo;
                    rev[i].AwayIconLink = fixtures[i].AwayTeam.Logo;
                    var raw_form_home = await _uow.GetRepository<Fixtures>().QueryToListAsync(
                   f => f.HomeTeamId == rev[i].HomeTeamId && f.StatusShort == "FT", f => f.DateUtc, 5,cancellationToken, f => f.HomeTeam, f => f.AwayTeam);
                    var raw_form_away = await _uow.GetRepository<Fixtures>().QueryToListAsync(
                        f => f.AwayTeamId == rev[i].AwayTeamId && f.StatusShort == "FT", f => f.DateUtc, 5,cancellationToken, f => f.HomeTeam, f => f.AwayTeam);
                    for (int j = 0; j < raw_form_home.Count; j++)
                    {
                        if (raw_form_home[j].HomeScore > raw_form_home[j].AwayScore)
                            rev[i].FormBadges[j].HomeBadge = "W";
                        if (raw_form_home[j].AwayScore > raw_form_home[j].HomeScore)
                            rev[i].FormBadges[j].HomeBadge = "L";
                        if (raw_form_home[j].AwayScore == raw_form_home[j].HomeScore)
                            rev[i].FormBadges[j].HomeBadge = "D";
                    }

                    for (int j = 0; j < raw_form_away.Count; j++)
                    {
                        if (raw_form_away[j].HomeScore > raw_form_away[j].AwayScore)
                            rev[i].FormBadges[j].AwayBadge = "W";
                        if (raw_form_away[j].AwayScore > raw_form_away[j].HomeScore)
                            rev[i].FormBadges[j].AwayBadge = "L";
                        if (raw_form_away[j].AwayScore == raw_form_away[j].HomeScore)
                            rev[i].FormBadges[j].AwayBadge = "D";
                    }


                }
            }
            else
            {

            }


                return rev;


        }
    }
}

using AutoMapper;
using GoallyticsApp.Application.Dtos;
using GoallyticsApp.Application.Dtos.ExternalApi;
using GoallyticsApp.Application.Dtos.Match;
using GoallyticsApp.Application.Features.CQRS.Commands;
using GoallyticsApp.Domain.Entities.AuthEntities;
using GoallyticsApp.Domain.Entities.MatchEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, CheckUserResponseDto>().ReverseMap();
            CreateMap<Gender,GenderDto>().ReverseMap();
            CreateMap<Fixtures,GetAllFixturesDto>().ReverseMap();
            CreateMap<Leagues, GetLeaguesDto>().ReverseMap();
            CreateMap<FixtureStat,StatisticH2HDto>().ReverseMap();
            CreateMap<Team, RequestTeamDto>().ReverseMap();
            CreateMap<Fixtures, DetailsStatDto>().ReverseMap();
            CreateMap<Fixtures,H2hMatchDto>().ReverseMap();
            CreateMap<Fixtures,ExternalFixtureDto>().ReverseMap();
            //Cqrs
            CreateMap<AppUser, UpdateUserCommandRequest>()
                .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

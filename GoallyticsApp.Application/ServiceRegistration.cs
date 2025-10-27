using AutoMapper;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Application.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(opt =>
            { opt.AddProfiles(new List<Profile>
                {
                    new UserProfile()
                });
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
        }
       
    }
}

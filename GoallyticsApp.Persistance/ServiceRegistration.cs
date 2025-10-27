using GoallyticsApp.Application;
using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Persistance.Context;
using GoallyticsApp.Persistance.Repositories;
using GoallyticsApp.Persistance.Seed;
using GoallyticsApp.Persistance.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance
{
    public static class ServiceRegistration
    {
        
        public async static void AddPersistanceServices(this IServiceCollection services,IConfiguration configuration)
        {

            // Burada Persistance katmanına ait servisleri kaydedebilirsiniz
            // Örneğin, DbContext, Repository vb.
            services.AddDbContext<MatchContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Local"));
                options.EnableSensitiveDataLogging(false);
                options.EnableServiceProviderCaching();
                options.EnableDetailedErrors(false);
            }); 
            // Diğer servis kayıtları
            services.AddScoped<IUow, Uow>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddApplicationServices();
            await services.SeedIfNeededAsync(configuration, "GoallyticsApp.Persistance\\Seed\\Raw\\Sql");
            
        }
    }
}

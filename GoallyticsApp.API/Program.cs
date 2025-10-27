using ExternalAPI;
using GoallyticsApp.Application;
using GoallyticsApp.Application.Tools;
using GoallyticsApp.Persistance;
using GoallyticsApp.Prediction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Serilog;


namespace GoallyticsApp.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.File("logs/prediction-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            builder.Host.UseSerilog();
            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>
            {
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtTokenDefaults.ValidIssuer,
                    ValidAudience = JwtTokenDefaults.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.IssuerSigningKey)),
                    ClockSkew=TimeSpan.Zero
                };
            });
            builder.Services.AddAuthorization();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.NumberHandling =
            JsonNumberHandling.AllowNamedFloatingPointLiterals;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            #region
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistanceServices(builder.Configuration);
            builder.Services.AddExternalAPIServices(builder.Configuration); // Update fixture service
            builder.Services.AddPredictionServices(builder.Configuration); // Prediction service
            #endregion


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
            });
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty; // root olarak açýlýr
                });
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}

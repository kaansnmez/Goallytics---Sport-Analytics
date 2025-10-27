
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.Json.Serialization;

namespace GoallyticsApp.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.LoginPath = "/Auth/Login";
                opt.LogoutPath = "/Auth/Logout";
                opt.AccessDeniedPath = "/Auth/AccessDenied";
                opt.Cookie.Name = "UdemyJwtAppCookie";
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                opt.SlidingExpiration = true;
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.NumberHandling =
                JsonNumberHandling.AllowNamedFloatingPointLiterals;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.Run();
        }
    }
}

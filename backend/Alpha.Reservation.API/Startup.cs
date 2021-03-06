using System;
using Alpha.Reservation.API.Extensions;
using Alpha.Reservation.API.Middleware;
using Alpha.Reservation.API.Options;
using Alpha.Reservation.App.Extensions;
using Alpha.Reservation.App.JwtAuthentication.Options;
using Alpha.Reservation.Data.Extensions;
using Alpha.Reservation.Data.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Alpha.Reservation.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddApiLayer(a =>
                {
                    a.JwtAuthenticationOptions = new JwtAuthenticationOptions
                    {
                        SecurityKey = _configuration["JwtSettings:SecurityKey"],
                        Issuer = _configuration["JwtSettings:Issuer"]
                    };
                })
                .AddAppLayer(a =>
                {
                    a.JwtTokenOptions = new JwtTokenOptions
                    {
                        SecurityKey = _configuration["JwtSettings:SecurityKey"],
                        Issuer = _configuration["JwtSettings:Issuer"],
                        LifeTime = TimeSpan.FromDays(7)
                    };
                })
                .AddDataLayer(a =>
                {
                    a.DbOptions = new DbOptions
                    {
                        DbConnection = _configuration["SqlServerDbSettings:DbConnection"],
                        DbMigrationAssembly = _configuration["SqlServerDbSettings:DbMigrationAssembly"]
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwagger()
                .UseSwaggerUI(a =>
                {
                    a.SwaggerEndpoint("/swagger/v1/swagger.json", "Alpha API V1");
                    a.RoutePrefix = "api/help";
                })
                .UseCors("CorsPolicy")
                .UseHttpsRedirection()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware(typeof(ErrorHandlingMiddleware))
                .UseEndpoints(a => a.MapControllers());
        }
    }
}
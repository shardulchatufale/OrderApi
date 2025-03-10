using ecommerce.sharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.sharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, IConfiguration config, string filename) where TContext : DbContext
        {
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("eCommerceConnection"), sqlserverOption =>
                sqlserverOption.EnableRetryOnFailure()));

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Debug().WriteTo.Console().WriteTo.File(path: $"{filename}-.text", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}[{Level:u3}]{message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day).CreateLogger();

            JWTAuthenticationScheme.AddJWTAuthanticationScheme(services, config);


            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
          //  app.UseMiddleware<ListenToOnlyApiGetway>();
            return app;
        }
    }
}

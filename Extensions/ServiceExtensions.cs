
using CodeFood_API.Asnan.DatabaseInitializer;
using CodeFood_API.Asnan.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {

            var host = config["DBHOST"] ?? config.GetConnectionString("MYSQL_HOST");
            var port = config["DBPORT"] ?? config.GetConnectionString("MYSQL_PORT");
            var password = config["MYSQL_PASSWORD"] ?? config.GetConnectionString("MYSQL_PASSWORD");
            var userid = config["MYSQL_USER"] ?? config.GetConnectionString("MYSQL_USER");
            var usersDataBase = config["MYSQL_DBNAME"] ?? config.GetConnectionString("MYSQL_DBNAME");

            var connectionString = $"server={host}; userid={userid};pwd={password};port={port};database={usersDataBase}";
            services.AddDbContext<ApplicationDbContext>(o => o.UseMySql(connectionString,
                MySqlServerVersion.LatestSupportedServerVersion));
        }

    }
}

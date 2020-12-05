using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using IdentityServer.Admin.SeedData;
using Serilog;
using Serilog.Events;

namespace IdentityServer.Admin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string logTemplate = "Logs/{0}/{1}.txt", outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}";
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Logger(x => x.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(string.Format(logTemplate, today, "Error"), outputTemplate: outputTemplate))
                .WriteTo.Logger(x => x.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                    .WriteTo.File(string.Format(logTemplate, today, "Warning"), outputTemplate: outputTemplate))
                .WriteTo.Logger(x => x.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(string.Format(logTemplate, today, "Information"), outputTemplate: outputTemplate))
                .CreateLogger();

            var configuration = GetConfiguration();

            var host = BuildWebHost(configuration, args);

            await host.InsertIdentitySeedData();
            await host.InsertIdentityServerSeedData();

            host.Run();
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();

        private static IConfiguration GetConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("identityserverdata.json", true, true)
                .AddJsonFile($"identityserverdata.{environment}.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}

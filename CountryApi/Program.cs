using System;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sentry;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.Loki;

namespace CountryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    true)
                .AddEnvironmentVariables()
                .Build();

            using (SentrySdk.Init(configuration.GetSection("ThirdParty").GetSection("Sentry")["ConnectionUrl"]))
            {
                ConfigureLogging(environment, configuration);

                CreateHostBuilder(args, configuration).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configuration)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.UseSerilog();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }

        public static void ConfigureLogging(string environment, IConfigurationRoot configuration)
        {
            var lokiCredentials = new NoAuthCredentials(configuration.GetSection("Logging").GetSection("Loki")["Uri"]);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.LokiHttp(lokiCredentials)
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using IdentityServer.Api.Extensions;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Infrastructure.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace IdentityServer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
       
            var identitySettings = Configuration.GetSection("IdentitySettings").Get<IdentitySettings>();
            
            DatabaseInitializer.InitializeAuthData(host.Services);
            DatabaseInitializer.InitializeUsersData(host.Services, identitySettings);
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(opts =>
                        {
                            var configuration = opts.ApplicationServices.GetService<IConfiguration>();
                            var hostenv = opts.ApplicationServices.GetService<IHostEnvironment>();
                            opts.Listen(IPAddress.Loopback, 5000);
                            opts.Listen(IPAddress.Loopback, 5001, listenOptions =>
                            {
                                listenOptions.UseHttps(
                                    new X509Certificate2(IServiceCollectionExtensions.GetCerfificate(hostenv)));
                            });
                        });
                    webBuilder.UseStartup<Startup>();
                });
        
        /// <summary>
        /// Configuration
        /// </summary>
        public static IConfigurationRoot Configuration { get; } = 
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .AddEnvironmentVariables()
                .Build();
    }
}

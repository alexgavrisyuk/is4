using System;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Infrastructure.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
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

using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using IdentityServer.Domain;
using IdentityServer.Infrastructure;
using IdentityServer.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace IdentityServer.Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = configuration.GetConnectionString("DefaultDb");

            services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); });
            
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.Password.RequireDigit = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequiredLength = 6;  
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                // .AddInMemoryApiResources(Config.GetApiResources())
                // .AddInMemoryClients(Config.GetClients())
                // .AddTestUsers()
                // .AddDeveloperSigningCredential()
                .AddSigningCredential(GetCerfificate(environment))
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                });
            
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Identity.Cookie";
            });
        }

        public static X509Certificate2 GetCerfificate(IHostEnvironment _env)
        {
            X509Certificate2 cert;
            cert = new X509Certificate2(Path.Combine(_env.ContentRootPath, "IdentityServer4Auth.pfx"), "");
            Log.Logger.Information($"Falling back to cert from file. Successfully loaded: {cert.Thumbprint}");

            return cert;
        }
    }
    
    //todo: client code
    // public static class ServiceCollectionExtensions
    // {
    //     public static void AddAuth(this IServiceCollection services, IHostEnvironment hostEnvironment)
    //     {
    //         JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    //         //
    //         
    //         services.AddAuthentication(options =>
    //             {
    //                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //             })
    //             .AddJwtBearer(options =>
    //             {
    //                 // if don`t specified, token parameters should be presented (offline auth)
    //                 // options.Authority = "http://localhost:5000/";
    //
    //                 // name of the API resource
    //                 options.Audience = "api1";
    //
    //
    //                 options.RequireHttpsMetadata = false;
    //
    //                 options.SaveToken = true;
    //                 
    //                 
    //                 options.TokenValidationParameters = new TokenValidationParameters()
    //                 {
    //                     ValidateIssuer = true,
    //                     ValidateIssuerSigningKey = true,
    //                     ValidIssuer = "http://localhost:5000",
    //                     // IssuerSigningKey = new X509SecurityKey(new X509Certificate2(Path.Combine(hostEnvironment.ContentRootPath, "myapp.pfx"), "testcert"))
    //                     IssuerSigningKey = new X509SecurityKey(new X509Certificate2(Path.Combine(hostEnvironment.ContentRootPath, "IdentityServer4Auth.cer"), ""))
    //                 };
    //             });
    //     }
}
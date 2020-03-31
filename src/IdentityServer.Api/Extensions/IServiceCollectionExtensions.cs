using System.Reflection;
using IdentityServer.Domain;
using IdentityServer.Infrastructure;
using IdentityServer.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
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
                .AddDeveloperSigningCredential()
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
            
            // services.AddAuthorization(o =>
            // {
            //     o.DefaultPolicy = new AuthorizationPolicyBuilder()
            //         .RequireAuthenticatedUser().Build();
            // });
            //
            // services.AddAuthentication("Cookie")
            //     .AddCookie("Cookie", config =>
            //     {
            //         config.Cookie.Name = "Cookie";
            //     });
        }
    }
}
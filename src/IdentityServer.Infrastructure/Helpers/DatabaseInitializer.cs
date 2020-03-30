using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Infrastructure.Enums;
using IdentityServer.Infrastructure.Options;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Infrastructure.Helpers
{
    public static class DatabaseInitializer
    {
        public static void InitializeAuthData(IServiceProvider app)
        {
            using (var serviceScope = app.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client);
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource);
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource);
                    }
                    context.SaveChanges();
                }
            }
        }

        public static void InitializeUsersData(IServiceProvider app, IdentitySettings settings)
        {
            using (var scope = app.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                TryMigrate(scope);
                TrySeedRoles(scope);
#if DEBUG
                TrySeedLocalDevelopmentEnvironmentAsync(scope).GetAwaiter().GetResult();
#endif   
            }
        }
        
        private static void TryMigrate(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
        
        private static void TrySeedRoles(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = roleManager.Roles?.ToList() ?? new List<IdentityRole>();

            var appRoles = new List<IdentityRole>
            {
                new IdentityRole(ApplicationRole.User.ToString()),
                new IdentityRole(ApplicationRole.Member.ToString()),
                new IdentityRole(ApplicationRole.Moderator.ToString()),
                new IdentityRole(ApplicationRole.Admin.ToString()),
                new IdentityRole(ApplicationRole.Owner.ToString())
            };

            appRoles.Where(t => roles.All(x => x.Name != t.Name))
                .ToList()
                .ForEach(role => roleManager.CreateAsync(role).GetAwaiter().GetResult());
        }

        private static async Task TrySeedLocalDevelopmentEnvironmentAsync(IServiceScope scope)
        {
            await Task.CompletedTask;
        }
    }
}
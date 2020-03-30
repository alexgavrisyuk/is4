using System.Collections.Generic;
using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.Infrastructure.Helpers
{
    public static class Config
    {
        public static List<Client> GetClients()
        {
            return new List<Client>();
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>();
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>();
        }
    }
}
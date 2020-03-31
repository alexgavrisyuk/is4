using System;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using ApiResource = IdentityServer4.Models.ApiResource;
using Client = IdentityServer4.Models.Client;
using IdentityResource = IdentityServer4.Models.IdentityResource;

namespace IdentityServer.Infrastructure.Helpers
{
    public static class Config
    {
        public static List<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientId = "account.service.client",

                    AllowedGrantTypes = new List<string>
                    {
                        OidcConstants.GrantTypes.Implicit
                    },    

                    RequireConsent = false,
                    RequirePkce = true,
                    
                    RequireClientSecret = false,

                    AccessTokenLifetime = (int)TimeSpan.FromMinutes(30).TotalSeconds,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                    },
                    
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:3000/callback",
                        "https://localhost:5002/signin-oidc",
                        "http://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:3000/index.html"
                    },
                }
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "Account Service")
            };
        }
    }
}
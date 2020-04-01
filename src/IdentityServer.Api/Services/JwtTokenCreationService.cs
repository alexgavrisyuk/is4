using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Api.Services
{
    public class JwtTokenCreationService: DefaultTokenCreationService
    {
        private readonly IHostEnvironment _environment;
        public JwtTokenCreationService(
            ISystemClock clock, 
            IKeyMaterialService keys, 
            IdentityServerOptions options,
            ILogger<DefaultTokenCreationService> logger, IHostEnvironment environment) 
            : base(clock, keys, options, logger)
        {
            _environment = environment;
        }

        public override async Task<string> CreateTokenAsync(Token token)
        {
            if (token.Type == IdentityServerConstants.TokenTypes.AccessToken)
            {
                var payload = await base.CreatePayloadAsync(token);

                var handler = new JsonWebTokenHandler();
                var jwe = handler.CreateToken(
                    payload.SerializeToJson(),
                    await Keys.GetSigningCredentialsAsync(),

                    // hardcoded... instead load public key per client
                    new X509EncryptingCredentials(new X509Certificate2(Path.Combine(_environment.ContentRootPath, "myapp.pfx"), "testcert"))); 

                return jwe;
            }

            return await base.CreateTokenAsync(token);
        }
    }
}
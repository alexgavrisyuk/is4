using IdentityServer.Core.Dtos;
using MediatR;

namespace IdentityServer.Core.Commands
{
    public class LoginCommand : IRequest<bool>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
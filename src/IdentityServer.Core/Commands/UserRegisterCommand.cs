using IdentityServer.Core.Dtos;
using MediatR;

namespace IdentityServer.Core.Commands
{
    public class UserRegisterCommand : IRequest<UserDto>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string PhoneNumber { get; set; }
    }
}
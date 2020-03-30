using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Core.Commands;
using IdentityServer.Core.Dtos;
using IdentityServer.Domain;
using IdentityServer.Infrastructure.Enums;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Core.CommandHandlers
{
    public class UserCommandsHandler : 
        IRequestHandler<UserRegisterCommand, UserDto>,
        IRequestHandler<LoginCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserCommandsHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(",", result.Errors.SelectMany(e => e.Description)));
            }

            var addToRoleResult =await _userManager.AddToRoleAsync(user, ApplicationRole.User.ToString());
            
            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new InvalidOperationException(addToRoleResult.Errors.ToString());
            }

            return user.Adapt<UserDto>();
        }

        public async Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);
            return result.Succeeded;
        }
    }
}
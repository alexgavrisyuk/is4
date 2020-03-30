using System.Threading.Tasks;
using IdentityServer.Core.Commands;
using IdentityServer.Core.Dtos;
using IdentityServer.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(IMediator mediator, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] UserRegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> LoginAsync([FromBody] LoginCommand command)
        {
            // var result = await _mediator.Send(command);
            var result = await _signInManager.PasswordSignInAsync(command.UserName, command.Password, true, false);
            return Ok(result);
        }
    }
}

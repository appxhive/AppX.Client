using AppX.Client.Application.BusinessCore.UserAccounts.Commands.EnableAuthenticator;
using AppX.Client.Application.BusinessCore.UserAccounts.Commands.LoginUser;
using AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.EnableAuthenticator;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.GetUserConfirmation;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.SignOut;
using AppX.Client.Domain.Interfaces.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppX.Client.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserAccountsController(IMediator mediator, 
        IIdentityService identityService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<bool>> Register(RegisterUserCommand command)
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await mediator.Send(new UserEmailConfirmationQuery(userId, token));

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(LoginUserCommand model)
        {
            var result = await mediator.Send(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult> EnableAuthenticator()
        {
            var IsSucceeded = await mediator.Send(new EnableAuthenticatorQuery());

            if (IsSucceeded) return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> EnableAuthenticator(EnableAuthenticatorCommand request)
        {
            var result = await mediator.Send(request);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async new Task<ActionResult> SignOut()
        {
            var result = await mediator.Send(new SignOutUserQuery());

            if (result.Success) return Ok();

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

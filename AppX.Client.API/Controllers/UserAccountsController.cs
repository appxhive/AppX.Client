using AppX.Client.Application.BusinessCore.UserAccounts.Commands.EnableAuthenticator;
using AppX.Client.Application.BusinessCore.UserAccounts.Commands.ForgotPassword;
using AppX.Client.Application.BusinessCore.UserAccounts.Commands.LoginUser;
using AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser;
using AppX.Client.Application.BusinessCore.UserAccounts.Commands.ResetPassword;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.EnableAuthenticator;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.ForgotPassword;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.GetUserConfirmation;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.ResetPassword;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.SignOut;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppX.Client.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserAccountsController(IMediator mediator) : ControllerBase
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
        public async Task<ActionResult> SignIn(LoginUserCommand command)
        {
            var result = await mediator.Send(command);

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
        public async Task<ActionResult> EnableAuthenticator(EnableAuthenticatorCommand command)
        {
            var result = await mediator.Send(command);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async new Task<ActionResult> SignOut()
        {
            var result = await mediator.Send(new SignOutUserQuery());

            if (result.Success) return Ok();

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword([FromQuery] string query = null)
        {
            var result = await mediator.Send(new ResetPasswordQuery
            {
                Code = query
            });

            return StatusCode((int)result.StatusCode, result);
        }

        //Reset Password is not currently working - QueryString value from IHttpContextAccessor is always empty after upgrading the .NET SDK from .NET6 to .NET 8
        //Might get different result if tested with a working Client UI where confirmation password is parsed and new password is inputted in the form by the user - Fortgot password --> Reset Password
        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand command) //Pass the code and new password
        {
            var result = await mediator.Send(command);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult> ForgotPassword()
        {
            var result = await mediator.Send(new ForgotPasswordQuery());

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var result = await mediator.Send(command);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

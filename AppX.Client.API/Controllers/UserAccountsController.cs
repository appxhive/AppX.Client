using AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser;
using AppX.Client.Application.BusinessCore.UserAccounts.Queries.GetUserConfirmation;
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
            var result = await mediator.Send(new GetUserConfirmationQuery(userId, token));

            return StatusCode((int)result.StatusCode, result);
        }
    }
}

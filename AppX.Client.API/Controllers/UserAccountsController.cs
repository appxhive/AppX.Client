using AppX.Client.Application.BusinessCore.UserAccounts.Commands.RegisterUser;
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
            var register = await mediator.Send(command);

            return Ok(register);
        }
    }
}

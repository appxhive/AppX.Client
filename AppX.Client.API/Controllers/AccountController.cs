using AppX.Client.Application.BusinessCore.IdentityUsers.Commands.RegisterIdentityUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppX.Client.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<bool>> Register(RegisterIdentityUserCommand command)
        {
            var isRegistered = await mediator.Send(command);

            if (isRegistered)
            {
                return Ok(true);
            }

            return BadRequest();
        }
    }
}

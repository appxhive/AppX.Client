using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppX.Client.API.Controllers
{
    //Controller for creating, reading, updating, deleting a client(i.e: School, Business entity)
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientsController(IMediator mediator) : ControllerBase
    {
        //Create Client
        //Read Client
        //Update Client
        //Delete Client
    }
}

using System.Threading.Tasks;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.User
{
    [ApiController]
    [Route("game/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

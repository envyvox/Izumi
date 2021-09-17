using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Models;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game
{
    [ApiController, Route("game/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:long}")]
        public async Task<ActionResult<UserDto>> GetUser([FromRoute] long id)
        {
            return Ok(await _mediator.Send(new GetUserQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            return Ok(await _mediator.Send(new GetUsersQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

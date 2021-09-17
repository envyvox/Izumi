using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.World.Commands;
using Izumi.Services.Game.World.Models;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game
{
    [ApiController, Route("game/world")]
    public class WorldController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorldController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("properties")]
        public async Task<ActionResult<List<WorldPropertyDto>>> GetWorldProperties()
        {
            return Ok(await _mediator.Send(new GetWorldPropertiesQuery()));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<WorldPropertyDto>> UpdateWorldProperty(UpdateWorldPropertyCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

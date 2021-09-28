using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.Building.Commands;
using Izumi.Services.Game.Building.Models;
using Izumi.Services.Game.Building.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game
{
    [ApiController, Route("game/buildings")]
    public class BuildingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<BuildingDto>>> GetBuildings()
        {
            return Ok(await _mediator.Send(new GetBuildingsQuery()));
        }

        [HttpGet, Route("{type:long}")]
        public async Task<ActionResult<BuildingDto>> GetBuilding([FromRoute] long type)
        {
            return Ok(await _mediator.Send(new GetBuildingQuery((BuildingType) type)));
        }

        [HttpPost]
        public async Task<ActionResult<BuildingDto>> UpdateBuilding(UpdateBuildingCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

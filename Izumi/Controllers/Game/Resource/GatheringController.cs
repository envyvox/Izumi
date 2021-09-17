using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Gathering.Commands;
using Izumi.Services.Game.Gathering.Models;
using Izumi.Services.Game.Gathering.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/gathering")]
    public class GatheringController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GatheringController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<GatheringDto>> GetGathering([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetGatheringQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<GatheringDto>>> GetGatherings()
        {
            return Ok(await _mediator.Send(new GetGatheringsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<GatheringDto>> CreateGathering(CreateGatheringCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<GatheringDto>> UpdateGathering(UpdateGatheringCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet, Route("{id:guid}/properties")]
        public async Task<ActionResult<List<GatheringPropertyDto>>> GetGatheringProperties([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetGatheringPropertiesQuery(id)));
        }

        [HttpPost, Route("properties/update")]
        public async Task<ActionResult<GatheringPropertyDto>> UpdateGatheringProperty(
            UpdateGatheringPropertyCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Fish.Commands;
using Izumi.Services.Game.Fish.Models;
using Izumi.Services.Game.Fish.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/fish")]
    public class FishController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<FishDto>> GetFish([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetFishQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<FishDto>>> GetFishes()
        {
            return Ok(await _mediator.Send(new GetFishesQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<FishDto>> CreateFish(CreateFishCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<FishDto>> UpdateFish(UpdateFishCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

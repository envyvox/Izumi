using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Seed.Commands;
using Izumi.Services.Game.Seed.Models;
using Izumi.Services.Game.Seed.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/seed")]
    public class SeedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<SeedDto>> GetSeed([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetSeedQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<SeedDto>>> GetSeeds()
        {
            return Ok(await _mediator.Send(new GetSeedsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<SeedDto>> CreateSeed(CreateSeedCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<SeedDto>> UpdateSeed(UpdateSeedCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

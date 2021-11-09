using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Crafting.Commands;
using Izumi.Services.Game.Crafting.Models;
using Izumi.Services.Game.Crafting.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/crafting")]
    public class CraftingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CraftingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<CraftingDto>> GetCrafting([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetCraftingByIdQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<CraftingDto>>> GetCraftings()
        {
            return Ok(await _mediator.Send(new GetCraftingsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<CraftingDto>> CreateCrafting(CreateCraftingCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<CraftingDto>> UpdateCrafting(UpdateCraftingCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet, Route("{id:guid}/properties")]
        public async Task<ActionResult<List<CraftingPropertyDto>>> GetCraftingProperties([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetCraftingPropertiesQuery(id)));
        }

        [HttpPost, Route("properties/update")]
        public async Task<ActionResult<CraftingPropertyDto>> UpdateCraftingProperty(
            UpdateCraftingPropertyCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

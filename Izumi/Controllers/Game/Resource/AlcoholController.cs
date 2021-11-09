using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Alcohol.Commands;
using Izumi.Services.Game.Alcohol.Models;
using Izumi.Services.Game.Alcohol.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/alcohol")]
    public class AlcoholController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlcoholController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<AlcoholDto>> GetAlcohol([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetAlcoholByIdQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<AlcoholDto>>> GetAlcohols()
        {
            return Ok(await _mediator.Send(new GetAlcoholsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<AlcoholDto>> CreateAlcohol(CreateAlcoholCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<AlcoholDto>> UpdateAlcohol(UpdateAlcoholCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet, Route("{id:guid}/properties")]
        public async Task<ActionResult<List<AlcoholPropertyDto>>> GetAlcoholProperties([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetAlcoholPropertiesQuery(id)));
        }

        [HttpPost, Route("properties/update")]
        public async Task<ActionResult<AlcoholPropertyDto>> UpdateAlcoholProperty(UpdateAlcoholPropertyCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

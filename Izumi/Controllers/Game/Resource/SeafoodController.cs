using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Seafood.Commands;
using Izumi.Services.Game.Seafood.Models;
using Izumi.Services.Game.Seafood.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/seafood")]
    public class SeafoodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeafoodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<SeafoodDto>> GetSeafood([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetSeafoodQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<SeafoodDto>>> GetSeafoods()
        {
            return Ok(await _mediator.Send(new GetSeafoodsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<SeafoodDto>> CreateSeafood(CreateSeafoodCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<SeafoodDto>> UpdateSeafood(UpdateSeafoodCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

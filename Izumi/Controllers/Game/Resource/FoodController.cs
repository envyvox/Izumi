using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Food.Commands;
using Izumi.Services.Game.Food.Models;
using Izumi.Services.Game.Food.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/food")]
    public class FoodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<FoodDto>> GetFood([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetFoodQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<FoodDto>>> GetFoods()
        {
            return Ok(await _mediator.Send(new GetFoodsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<FoodDto>> CreateFood(CreateFoodCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<FoodDto>> UpdateFood(UpdateFoodCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

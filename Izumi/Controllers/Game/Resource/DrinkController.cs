using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Drink.Commands;
using Izumi.Services.Game.Drink.Models;
using Izumi.Services.Game.Drink.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/drink")]
    public class DrinkController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DrinkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<DrinkDto>> GetDrink([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetDrinkQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<DrinkDto>>> GetDrinks()
        {
            return Ok(await _mediator.Send(new GetDrinksQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<DrinkDto>> CreateDrink(CreateDrinkCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<DrinkDto>> UpdateDrink(UpdateDrinkCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

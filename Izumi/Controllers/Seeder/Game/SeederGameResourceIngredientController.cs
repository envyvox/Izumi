using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Game;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController, Route("seeder/game/resource/ingredient")]
    public class SeederGameResourceIngredientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGameResourceIngredientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("alcohol")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAlcoholIngredients()
        {
            return Ok(await _mediator.Send(new SeederUploadAlcoholIngredientsCommand()));
        }

        [HttpPost, Route("crafting")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCraftingIngredients()
        {
            return Ok(await _mediator.Send(new SeederUploadCraftingIngredientsCommand()));
        }

        [HttpPost, Route("food")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadFoodIngredients()
        {
            return Ok(await _mediator.Send(new SeederUploadFoodIngredientsCommand()));
        }
    }
}

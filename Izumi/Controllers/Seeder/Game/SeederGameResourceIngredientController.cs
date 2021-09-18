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
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAlcoholIngredients(
            SeederUploadAlcoholIngredientsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("crafting")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCraftingIngredients(
            SeederUploadCraftingIngredientsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("food")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadFoodIngredients(
            SeederUploadFoodIngredientsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

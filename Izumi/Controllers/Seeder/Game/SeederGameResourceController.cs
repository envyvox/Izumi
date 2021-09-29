using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Game;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController, Route("seeder/game/resource")]
    public class SeederGameResourceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGameResourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("alcohols")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAlcohols()
        {
            return Ok(await _mediator.Send(new SeederUploadAlcoholsCommand()));
        }

        [HttpPost, Route("craftings")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCraftings()
        {
            return Ok(await _mediator.Send(new SeederUploadCraftingsCommand()));
        }

        [HttpPost, Route("crops")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCrops()
        {
            return Ok(await _mediator.Send(new SeederUploadCropsCommand()));
        }

        [HttpPost, Route("drinks")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadDrinks()
        {
            return Ok(await _mediator.Send(new SeederUploadDrinksCommand()));
        }

        [HttpPost, Route("fishes")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadFishes()
        {
            return Ok(await _mediator.Send(new SeederUploadFishesCommand()));
        }

        [HttpPost, Route("foods")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadFoods()
        {
            return Ok(await _mediator.Send(new SeederUploadFoodsCommand()));
        }

        [HttpPost, Route("gatherings")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadGatherings()
        {
            return Ok(await _mediator.Send(new SeederUploadGatheringsCommand()));
        }

        [HttpPost, Route("products")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadProducts()
        {
            return Ok(await _mediator.Send(new SeederUploadProductsCommand()));
        }

        [HttpPost, Route("seafoods")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadSeafoods()
        {
            return Ok(await _mediator.Send(new SeederUploadSeafoodsCommand()));
        }

        [HttpPost, Route("seeds")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadSeeds()
        {
            return Ok(await _mediator.Send(new SeederUploadSeedsCommand()));
        }
    }
}

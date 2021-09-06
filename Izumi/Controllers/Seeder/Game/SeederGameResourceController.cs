using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Game;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController]
    [Route("seeder/game/resource")]
    public class SeederGameResourceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGameResourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("alcohols")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAlcohols(SeederUploadAlcoholsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("craftings")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCraftings(SeederUploadCraftingsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("crops")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCrops(SeederUploadCropsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("drinks")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadDrinks(SeederUploadDrinksCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("fishes")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadFishes(SeederUploadFishesCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("foods")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadFoods(SeederUploadFoodsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("gatherings")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadGatherings(
            SeederUploadGatheringsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("products")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadProducts(SeederUploadProductsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("seafoods")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadSeafoods(SeederUploadSeafoodsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("seeds")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadSeeds(SeederUploadSeedsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

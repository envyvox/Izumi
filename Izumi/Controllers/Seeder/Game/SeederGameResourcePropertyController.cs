using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Game;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController, Route("seeder/game/resource/property")]
    public class SeederGameResourcePropertyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGameResourcePropertyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("alcohol")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAlcoholProperties()
        {
            return Ok(await _mediator.Send(new SeederUploadAlcoholPropertiesCommand()));
        }

        [HttpPost, Route("crafting")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCraftingProperties()
        {
            return Ok(await _mediator.Send(new SeederUploadCraftingPropertiesCommand()));
        }

        [HttpPost, Route("gathering")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadGatheringProperties()
        {
            return Ok(await _mediator.Send(new SeederUploadGatheringPropertiesCommand()));
        }
    }
}

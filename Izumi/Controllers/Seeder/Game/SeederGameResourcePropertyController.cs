using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Game;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController]
    [Route("seeder/game/resource/property")]
    public class SeederGameResourcePropertyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGameResourcePropertyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("alcohol")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAlcoholProperties(
            SeederUploadAlcoholPropertiesCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("crafting")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadCraftingProperties(
            SeederUploadCraftingPropertiesCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Route("gathering")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadGatheringProperties(
            SeederUploadGatheringPropertiesCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

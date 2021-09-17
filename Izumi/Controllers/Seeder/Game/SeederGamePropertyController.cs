using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Game;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController, Route("seeder/game/property")]
    public class SeederGamePropertyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGamePropertyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("world")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadWorldProperties(
            SeederUploadWorldPropertiesCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

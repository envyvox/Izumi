using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Discord;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Discord
{
    [ApiController]
    [Route("seeder/discord")]
    public class SeederDiscordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederDiscordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("images")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadImages(SeederUploadImagesCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

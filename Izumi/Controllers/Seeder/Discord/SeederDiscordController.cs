using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Discord;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Discord
{
    [ApiController, Route("seeder/discord")]
    public class SeederDiscordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederDiscordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("images")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadImages()
        {
            return Ok(await _mediator.Send(new SeederUploadImagesCommand()));
        }

        [HttpPost, Route("slash-commands")]
        public async Task<ActionResult> UploadSlashCommands()
        {
            return Ok(await _mediator.Send(new SeederUploadSlashCommandsCommand()));
        }
    }
}

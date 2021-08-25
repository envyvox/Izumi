using System.Threading.Tasks;
using Izumi.Services.Discord.SlashCommands.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Discord
{
    [ApiController]
    [Route("discord/commands")]
    public class CommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("upload-slash-commands")]
        public async Task<ActionResult> UploadSlashCommands()
        {
            return Ok(await _mediator.Send(new UploadSlashCommandsCommand()));
        }
    }
}

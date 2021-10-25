using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Discord.Emote.Commands;
using Izumi.Services.Discord.Emote.Models;
using Izumi.Services.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Discord
{
    [ApiController, Route("discord/emote")]
    public class EmoteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<Dictionary<string, EmoteDto>>> GetEmotes()
        {
            return Ok(await Task.FromResult(DiscordRepository.Emotes));
        }

        [HttpPost, Route("sync")]
        public async Task<ActionResult> SyncEmotes()
        {
            return Ok(await _mediator.Send(new SyncEmotesCommand()));
        }
    }
}

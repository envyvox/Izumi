using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Izumi.Services.Discord.Emote.Models;
using Izumi.Services.Discord.Emote.Queries;
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
            return Ok(await _mediator.Send(new GetEmotesQuery()));
        }

        [HttpPost, Route("upload")]
        public async Task<ActionResult> UploadEmotes()
        {
            RecurringJob.Trigger("upload-emotes");
            return await Task.FromResult(Ok());
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Localization.Commands;
using Izumi.Services.Game.Localization.Models;
using Izumi.Services.Game.Localization.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game
{
    [ApiController, Route("game/localization")]
    public class LocalizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocalizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<LocalizationDto>>> GetLocalizations()
        {
            return Ok(await _mediator.Send(new GetLocalizationsQuery()));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<LocalizationDto>> UpdateLocalization(UpdateLocalizationCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

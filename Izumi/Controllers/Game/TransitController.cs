using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.Transit.Models;
using Izumi.Services.Game.Transit.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game
{
    [ApiController, Route("game/transit")]
    public class TransitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<TransitDto>>> GetTransits()
        {
            return Ok(await _mediator.Send(new GetTransitsQuery()));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<TransitDto>> UpdateTransit(UpdateTransitCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Banner.Commands;
using Izumi.Services.Game.Banner.Models;
using Izumi.Services.Game.Banner.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game
{
    [ApiController, Route("game/banners")]
    public class BannerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BannerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("")]
        public async Task<ActionResult<List<BannerDto>>> GetBanners()
        {
            return Ok(await _mediator.Send(new GetBannersQuery()));
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<BannerDto>> GetBanner([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetBannerByIdQuery(id)));
        }

        [HttpPut, Route("")]
        public async Task<ActionResult<BannerDto>> CreateBanner(CreateBannerCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("")]
        public async Task<ActionResult<BannerDto>> UpdateBanner(UpdateBannerCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

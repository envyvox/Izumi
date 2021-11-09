using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Crop.Commands;
using Izumi.Services.Game.Crop.Models;
using Izumi.Services.Game.Crop.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/crop")]
    public class CropController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CropController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<CropDto>> GetCrop([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetCropByIdQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<CropDto>>> GetCrops()
        {
            return Ok(await _mediator.Send(new GetCropsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<CropDto>> CreateCrop(CreateCropCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<CropDto>> UpdateCrop(UpdateCropCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

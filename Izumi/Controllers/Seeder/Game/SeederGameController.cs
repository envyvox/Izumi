using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Discord;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController]
    [Route("seeder/game")]
    public class SeederGameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("banners")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadBanners(SeederUploadBannersCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

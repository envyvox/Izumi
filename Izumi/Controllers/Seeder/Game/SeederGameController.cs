using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Seeder.Game;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Seeder.Game
{
    [ApiController, Route("seeder/game")]
    public class SeederGameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeederGameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("achievements")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAchievements(SeederUploadAchievementsCommand
            request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("banners")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadBanners(SeederUploadBannersCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("localizations")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadLocalizations(
            SeederUploadLocalizationsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("transits")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadTransits(SeederUploadTransitsCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

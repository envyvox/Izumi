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
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadAchievements()
        {
            return Ok(await _mediator.Send(new SeederUploadAchievementsCommand()));
        }

        [HttpPost, Route("banners")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadBanners()
        {
            return Ok(await _mediator.Send(new SeederUploadBannersCommand()));
        }

        [HttpPost, Route("buildings")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadBuildings()
        {
            return Ok(await _mediator.Send(new SeederUploadBuildingsCommand()));
        }

        [HttpPost, Route("contracts")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadContacts()
        {
            return Ok(await _mediator.Send(new SeederUploadContractsCommand()));
        }

        [HttpPost, Route("localizations")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadLocalizations()
        {
            return Ok(await _mediator.Send(new SeederUploadLocalizationsCommand()));
        }

        [HttpPost, Route("transits")]
        public async Task<ActionResult<TotalAndAffectedCountDto>> UploadTransits()
        {
            return Ok(await _mediator.Send(new SeederUploadTransitsCommand()));
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Commands;
using Izumi.Services.Game.World.Models;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game
{
    [ApiController, Route("game/world")]
    public class WorldController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorldController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("properties/list")]
        public async Task<ActionResult<List<WorldPropertyDto>>> GetWorldProperties()
        {
            return Ok(await _mediator.Send(new GetWorldPropertiesQuery()));
        }

        [HttpPost, Route("properties/update")]
        public async Task<ActionResult<WorldPropertyDto>> UpdateWorldProperty(UpdateWorldPropertyCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet, Route("settings/current-season")]
        public async Task<ActionResult<SeasonType>> GetCurrentSeason()
        {
            return Ok(await _mediator.Send(new GetCurrentSeasonQuery()));
        }

        [HttpGet, Route("settings/current-timesday")]
        public async Task<ActionResult<TimesDayType>> GetCurrentTimesDay()
        {
            return Ok(await _mediator.Send(new GetCurrentTimesDayQuery()));
        }

        [HttpGet, Route("settings/weather-today")]
        public async Task<ActionResult<WeatherType>> GetWeatherToday()
        {
            return Ok(await _mediator.Send(new GetWeatherTodayQuery()));
        }

        [HttpGet, Route("settings/weather-tomorrow")]
        public async Task<ActionResult<WeatherType>> GetWeatherTomorrow()
        {
            return Ok(await _mediator.Send(new GetWeatherTomorrowQuery()));
        }

        [HttpPost, Route("settings/current-season")]
        public async Task<ActionResult> UpdateCurrentSeason(UpdateCurrentSeasonCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("settings/weather-today")]
        public async Task<ActionResult> UpdateWeatherToday(UpdateWeatherTodayCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("settings/weather-tomorrow")]
        public async Task<ActionResult> UpdateWeatherTomorrow(UpdateWeatherTomorrowCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}

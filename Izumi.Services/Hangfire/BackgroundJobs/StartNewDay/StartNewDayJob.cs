using System;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.Field.Commands;
using Izumi.Services.Game.World.Commands;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.StartNewDay
{
    public class StartNewDayJob : IStartNewDayJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StartNewDayJob> _logger;
        private readonly Random _random = new();

        public StartNewDayJob(
            IMediator mediator,
            ILogger<StartNewDayJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute()
        {
            await _mediator.Send(new MoveAllFieldsProgressCommand());

            var weatherToday = await GenerateWeather();

            await _mediator.Send(new UpdateAllFieldsStateCommand(weatherToday == WeatherType.Rain
                ? FieldStateType.Watered
                : FieldStateType.Planted));
        }

        private async Task<WeatherType> GenerateWeather()
        {
            _logger.LogInformation(
                "Generate weather executed");

            var chance = _random.Next(1, 101);
            var oldWeatherToday = await _mediator.Send(new GetWeatherTodayQuery());
            var newWeatherToday = await _mediator.Send(new GetWeatherTomorrowQuery());

            var newWeatherTomorrow = newWeatherToday == WeatherType.Clear
                ? chance + (oldWeatherToday == WeatherType.Rain ? 10 : 20) > 50
                    ? WeatherType.Rain
                    : WeatherType.Clear
                : chance + (oldWeatherToday == WeatherType.Clear ? 10 : 20) > 50
                    ? WeatherType.Clear
                    : WeatherType.Rain;

            await _mediator.Send(new UpdateWeatherTodayCommand(newWeatherToday));
            await _mediator.Send(new UpdateWeatherTomorrowCommand(newWeatherTomorrow));

            return newWeatherToday;
        }
    }
}

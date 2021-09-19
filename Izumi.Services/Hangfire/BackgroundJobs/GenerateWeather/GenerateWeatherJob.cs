using System;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Commands;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Hangfire.BackgroundJobs.GenerateWeather
{
    public class GenerateWeatherJob : IGenerateWeatherJob
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public GenerateWeatherJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute()
        {
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
        }
    }
}

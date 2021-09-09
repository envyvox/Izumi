using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User
{
    public record WorldInfoCommand(SocketSlashCommand Command) : IRequest;

    public class WorldInfoHandler : IRequestHandler<WorldInfoCommand>
    {
        private readonly IMediator _mediator;
        private readonly TimeZoneInfo _timeZoneInfo;

        public WorldInfoHandler(
            IMediator mediator,
            TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _timeZoneInfo = timeZoneInfo;
        }

        public async Task<Unit> Handle(WorldInfoCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
            var timesDay = await _mediator.Send(new GetCurrentTimesDayQuery());
            var weatherToday = await _mediator.Send(new GetWeatherTodayQuery());
            var weatherTomorrow = await _mediator.Send(new GetWeatherTomorrowQuery());
            var currentSeason = await _mediator.Send(new GetCurrentSeasonQuery());

            var embed = new EmbedBuilder()
                .WithAuthor("Информация о мире")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображается информация о мире:" +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Текущее время",
                    "*Время суток влияет на виды рыб, которые можно поймать.*" +
                    $"\n\n{emotes.GetEmote("Arrow")} Сейчас {timeNow:t}, **{timesDay.Localize()}**")
                .AddField("Погода сегодня",
                    "*Ежедневная погода влияет на виды рыб, которые можно поймать, " +
                    "а так же в дождливую погоду не нужно поливать урожай.*" +
                    $"\n\n{emotes.GetEmote("Arrow")} Сегодня погода будет **{weatherToday.Localize()}**")
                .AddField("Предсказательница",
                    "*C вами предсказательница, ваш источник прогнозов погоды номер один. А сейчас - прогноз погоды на завтра...*" +
                    $"\n\n{emotes.GetEmote("Arrow")} Погода обещает быть **{weatherTomorrow.Localize()}**")
                .AddField("Сезон",
                    "*Текущий сезон определяет ассортимент семян в магазине, ведь у каждого урожая есть свой сезон роста. " +
                    "Посаженные на ячейки семена умирают при смене сезона, поэтому будь дальновидным. " +
                    "Так же влияет на виды рыб, которые можно поймать.*" +
                    $"\n\n{emotes.GetEmote("Arrow")} Сейчас **{currentSeason.Localize().ToLower()}**")
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(
                    weatherToday == WeatherType.Clear
                        ? ImageType.WeatherClear
                        : ImageType.WeatherRain)))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.WorldInfo)));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

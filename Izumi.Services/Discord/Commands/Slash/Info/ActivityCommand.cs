using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Statistic.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Info
{
    public record ActivityCommand(SocketSlashCommand Command) : IRequest;

    public class ActivityCommandHandler : IRequestHandler<ActivityCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;

        public ActivityCommandHandler(
            IMediator mediator,
            ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<Unit> Handle(ActivityCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userStatistics = await _mediator.Send(new GetUserStatisticsQuery(user.Id));

            var userVoiceMinutes = userStatistics.ContainsKey(StatisticType.VoiceMinutes)
                ? userStatistics[StatisticType.VoiceMinutes].Amount
                : 0;

            var userMessages = userStatistics.ContainsKey(StatisticType.Messages)
                ? userStatistics[StatisticType.Messages].Amount
                : 0;

            var gameStatistics = Enum
                .GetValues(typeof(StatisticType))
                .Cast<StatisticType>()
                .Where(x =>
                    x is not StatisticType.Messages &&
                    x is not StatisticType.VoiceMinutes)
                .ToArray();

            var userGameStatistic = gameStatistics.Aggregate(string.Empty, (s, v) =>
                s + $"{emotes.GetEmote("List")} {v.Localize()}: **{(userStatistics.ContainsKey(v) ? userStatistics[v].Amount : 0)}**\n");

            var embed = new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображается вся информация о твоей активности на сервере в этом месяце:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Награды за активность начисляются первого числа каждого месяца." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Активность в чате",
                    $"**{userMessages}** {_localizationService.Localize(LocalizationCategoryType.Basic, "Message", userMessages)}",
                    true)
                .AddField("Голосовой онлайн",
                    $"{userVoiceMinutes.Minutes().Humanize(2, new CultureInfo("ru-RU"))}",
                    true)
                .AddEmptyField(true)
                .AddField("Игровая активность", userGameStatistic);

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

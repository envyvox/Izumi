using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Effect.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Effect.Commands
{
    public record StartLotteryCommand : IRequest;

    public class StartLotteryHandler : IRequestHandler<StartLotteryCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StartLotteryHandler> _logger;
        private readonly ILocalizationService _local;

        public StartLotteryHandler(
            IMediator mediator,
            ILogger<StartLotteryHandler> logger,
            ILocalizationService local)
        {
            _mediator = mediator;
            _logger = logger;
            _local = local;
        }

        public async Task<Unit> Handle(StartLotteryCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var reward = await _mediator.Send(new GetWorldPropertyValueQuery(WorldPropertyType.CasinoLotteryReward));
            var winner = await _mediator.Send(new GetRandomUserWithEffectQuery(EffectType.Lottery));
            var socketWinner = await _mediator.Send(new GetSocketGuildUserQuery((ulong) winner.Id));

            await _mediator.Send(new AddCurrencyToUserCommand(winner.Id, CurrencyType.Ien, reward));
            await _mediator.Send(new AddStatisticToUserCommand(winner.Id, StatisticType.CasinoLotteryWin));
            await _mediator.Send(new DeleteUsersEffectCommand(EffectType.Lottery));

            var embedPm = new EmbedBuilder()
                .WithDescription(
                    $"Твой {emotes.GetEmote("LotteryTicket")} лотерейный билет оказался счастливым и приносит тебе " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {reward} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), reward)}, " +
                    "трать с умом!");

            await _mediator.Send(new SendEmbedToUserCommand((ulong) winner.Id, embedPm));

            var embedNotify = new EmbedBuilder()
                .WithAuthor("Дорогой дневник,")
                .WithDescription(
                    "Все жители **столицы** только и говорят о везунчике " +
                    $"{emotes.GetEmote(winner.Title.EmoteName())} {winner.Title.Localize()} {socketWinner.Mention}, " +
                    $"который победил в {emotes.GetEmote("LotteryTicket")} лотерее и получил " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {reward} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), reward)}, " +
                    "вот бы и мне так везло...");

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannelType.GameDiary, embedNotify));

            _logger.LogInformation(
                "Lottery ended with winner {UserId}",
                winner.Id);

            return Unit.Value;
        }
    }
}

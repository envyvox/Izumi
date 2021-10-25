using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Effect.Commands;
using Izumi.Services.Game.Effect.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Casino
{
    public record LotteryBuyCommand(SocketSlashCommand Command) : IRequest;

    public class LotteryBuyHandler : IRequestHandler<LotteryBuyCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public LotteryBuyHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(LotteryBuyCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = DiscordRepository.Emotes;
            var hasLottery = await _mediator.Send(new CheckUserHasEffectQuery(user.Id, EffectType.Lottery));

            var embed = new EmbedBuilder();

            if (hasLottery)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя ведь уже есть {emotes.GetEmote("LotteryTicket")} лотерейный билет, зачем тебе еще один? " +
                    "Дождись розыгрыша а затем покупай новый.");
            }
            else
            {
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));
                var lotteryPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.CasinoLotteryPrice));

                if (userCurrency.Amount < lotteryPrice)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString())} " +
                        $"для приобретения {emotes.GetEmote("LotteryTicket")} лотерейного билета.");
                }
                else
                {
                    await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, lotteryPrice));
                    await _mediator.Send(new CreateUserEffectCommand(user.Id, EffectType.Lottery, null));
                    await _mediator.Send(new AddStatisticToUserCommand(user.Id, StatisticType.CasinoLotteryBuy));
                    await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                    {
                        AchievementType.FirstLotteryTicket,
                        AchievementType.Casino22LotteryBuy,
                        AchievementType.Casino99LotteryBuy
                    }));

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно приобрел {emotes.GetEmote("LotteryTicket")} лотерейный билет за " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {lotteryPrice} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), lotteryPrice)}.");
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

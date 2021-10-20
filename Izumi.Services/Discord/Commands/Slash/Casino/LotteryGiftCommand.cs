using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
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
    public record LotteryGiftCommand(SocketSlashCommand Command) : IRequest;

    public class LotteryGiftHandler : IRequestHandler<LotteryGiftCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public LotteryGiftHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(LotteryGiftCommand request, CancellationToken ct)
        {
            var targetSocketUser = (SocketGuildUser) request.Command.Data.Options.First().Options.First().Value;

            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var target = await _mediator.Send(new GetUserQuery((long) targetSocketUser.Id));

            var embed = new EmbedBuilder();

            if (target.Id == user.Id)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты конечно можешь заплатить курьерской службе чтобы они доставили тебе " +
                    $"{emotes.GetEmote("LotteryTicket")} лотерейный билет, но в этом нет никакого смысла, просто купи его.");
            }
            else if (targetSocketUser.IsBot)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"ты не можешь подарить {emotes.GetEmote("LotteryTicket")} лотерейный билет боту.");
            }
            else
            {
                var hasLottery = await _mediator.Send(new CheckUserHasEffectQuery(target.Id, EffectType.Lottery));

                if (hasLottery)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"у {emotes.GetEmote(target.Title.EmoteName())} {target.Title.Localize()} {targetSocketUser.Mention} " +
                        $"уже есть {emotes.GetEmote("LotteryTicket")} лотерейный билет, лучше подарить его кому-либо еще.");
                }
                else
                {
                    var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));
                    var lotteryPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.CasinoLotteryPrice));
                    var lotteryDeliveryPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.CasinoLotteryDeliveryPrice));

                    if (userCurrency.Amount < lotteryPrice + lotteryDeliveryPrice)
                    {
                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString())} " +
                            $"для оплаты {emotes.GetEmote("LotteryTicket")} лотерейного билета в подарок.");
                    }
                    else
                    {
                        await _mediator.Send(new RemoveCurrencyFromUserCommand(
                            user.Id, CurrencyType.Ien, lotteryPrice + lotteryDeliveryPrice));
                        await _mediator.Send(new CreateUserEffectCommand(target.Id, EffectType.Lottery, null));
                        await _mediator.Send(new AddStatisticToUserCommand(user.Id, StatisticType.CasinoLotteryGift));
                        await _mediator.Send(new CheckAchievementInUserCommand(
                            user.Id, AchievementType.Casino20LotteryGift));

                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"ты успешно отправил {emotes.GetEmote("LotteryTicket")} лотерейный билет " +
                            $"{emotes.GetEmote(target.Title.EmoteName())} {target.Title.Localize()} {targetSocketUser.Mention}. " +
                            "\nНаша курьерская служба доставит его сию секунду.");

                        var embedNotify = new EmbedBuilder()
                            .WithDescription(
                                $"Ты получил в подарок {emotes.GetEmote("LotteryTicket")} лотерейный билет от " +
                                $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}.");

                        await _mediator.Send(new SendEmbedToUserCommand(targetSocketUser.Id, embedNotify));
                    }
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

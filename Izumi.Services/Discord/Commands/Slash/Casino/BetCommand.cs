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
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Cooldown.Commands;
using Izumi.Services.Game.Cooldown.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Casino
{
    public record BetCommand(SocketSlashCommand Command) : IRequest;

    public class BetHandler : IRequestHandler<BetCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly Random _random = new();

        public BetHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(BetCommand request, CancellationToken cancellationToken)
        {
            var betAmount = (uint) (long) request.Command.Data.Options.First().Value;

            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var userCooldown = await _mediator.Send(new GetUserCooldownQuery(user.Id, CooldownType.CasinoBet));

            var embed = new EmbedBuilder();

            if (userCooldown.Expiration > DateTimeOffset.UtcNow)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "Прости, но кубики куда-то закатились, попробуй еще раз через " +
                    $"**{(userCooldown.Expiration - DateTimeOffset.UtcNow).Humanize(1, new CultureInfo("ru-RU"))}**.");
            }
            else
            {
                var minAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.CasinoBetMinAmount));
                var maxAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.CasinoBetMaxAmount));

                if (betAmount < minAmount)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"извини, но мы играем по-крупному! Не меньше {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                        $"{minAmount} {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), minAmount)}.");
                }
                else if (betAmount > maxAmount)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "ничего себе, у тебя крупная сумма! Но ты не мог бы разделить ставку по " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {maxAmount} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), maxAmount)} " +
                        "максимум? Иначе крупье может хватить удар от таких богатств.");
                }
                else
                {
                    var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

                    if (userCurrency.Amount < betAmount)
                    {
                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"прискорбно, но у тебя иссякли {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 2)} " +
                            "на подобную ставку. Не унывай! Двери нашего казино всегда открыты, возвращайся в любое время!");
                    }
                    else
                    {
                        double firstDrop = _random.Next(1, 101);
                        double secondDrop = _random.Next(1, 101);
                        var cubeDrop = Math.Floor((firstDrop + secondDrop) / 2);

                        var cubeDropString =
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"на кубиках выпадает **{cubeDrop}**.\n\n";

                        switch (cubeDrop)
                        {
                            case >= 55 and < 90:

                                cubeDropString +=
                                    "Прямо чувствуется, как повышается азарт от игры и выигранных " +
                                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {betAmount * 2} " +
                                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), betAmount * 2)}! " +
                                    "Главное, не теряй свое чувство меры!";

                                await _mediator.Send(new AddCurrencyToUserCommand(
                                    user.Id, CurrencyType.Ien, betAmount));

                                break;

                            case >= 90 and < 100:

                                cubeDropString +=
                                    "Прямо чувствуется, как повышается азарт от игры и выигранных " +
                                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {betAmount * 4} " +
                                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), betAmount * 4)}! " +
                                    "Главное, не теряй свое чувство меры!";

                                await _mediator.Send(new AddCurrencyToUserCommand(
                                    user.Id, CurrencyType.Ien, betAmount * 3));

                                break;

                            case 100:

                                cubeDropString +=
                                    "Прямо чувствуется, как повышается азарт от игры и выигранных " +
                                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {betAmount * 10} " +
                                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), betAmount * 10)}! " +
                                    "Главное, не теряй свое чувство меры!";

                                await _mediator.Send(new AddStatisticToUserCommand(
                                    user.Id, StatisticType.CasinoJackpot));
                                await _mediator.Send(new CheckAchievementInUserCommand(
                                    user.Id, AchievementType.FirstJackPot));
                                await _mediator.Send(new AddCurrencyToUserCommand(
                                    user.Id, CurrencyType.Ien, betAmount * 9));

                                break;

                            default:

                                cubeDropString +=
                                    $"Сожалеем, ты проиграл {emotes.GetEmote(CurrencyType.Ien.ToString())} {betAmount} " +
                                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), betAmount)}! " +
                                    "Не сильно вини дилера.";

                                await _mediator.Send(new RemoveCurrencyFromUserCommand(
                                    user.Id, CurrencyType.Ien, betAmount));

                                break;
                        }

                        var cooldown = await _mediator.Send(new GetWorldPropertyValueQuery(
                            WorldPropertyType.CasinoBetCooldownMinutes));

                        await _mediator.Send(new CreateUserCooldownCommand(
                            user.Id, CooldownType.CasinoBet, TimeSpan.FromMinutes(cooldown)));
                        await _mediator.Send(new AddStatisticToUserCommand(
                            user.Id, StatisticType.CasinoBet));
                        await _mediator.Send(new CheckAchievementsInUserCommand(
                            user.Id, new[]
                            {
                                AchievementType.Casino33Bet,
                                AchievementType.Casino777Bet
                            }));

                        embed.WithDescription(cubeDropString);
                    }
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

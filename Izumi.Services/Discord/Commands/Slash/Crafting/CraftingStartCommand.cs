using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Alcohol.Commands;
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Collection.Commands;
using Izumi.Services.Game.Crafting.Commands;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Drink.Commands;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Ingredient.Commands;
using Izumi.Services.Game.Ingredient.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Localization.Queries;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Crafting
{
    public record CraftingStartCommand(SocketSlashCommand Command) : IRequest;

    public class CraftingStartHandler : IRequestHandler<CraftingStartCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CraftingStartHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CraftingStartCommand request, CancellationToken ct)
        {
            var category = (string) request.Command.Data.Options
                .Single(x => x.Name == "категория").Value;
            var amount = (uint) (long) request.Command.Data.Options
                .Single(x => x.Name == "количество").Value;
            var name = (string) request.Command.Data.Options
                .Single(x => x.Name == "название").Value;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));
            var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EnergyCostMaking));

            var embed = new EmbedBuilder()
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Crafting)));

            switch (category)
            {
                case "предмет":
                {
                    user.Location.CheckRequiredLocation(LocationType.Seaport);

                    var craftingLocal = await _mediator.Send(new GetLocalizationByLocalizedNameQuery(
                        LocalizationCategoryType.Crafting, name));
                    var crafting = await _mediator.Send(new GetCraftingByNameQuery(craftingLocal.Name));
                    var costPrice = await _mediator.Send(new GetCraftingCostPriceQuery(crafting.Ingredients));
                    var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice, amount));

                    if (userCurrency.Amount < craftingPrice)
                    {
                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 5)} " +
                            "для оплаты изготовления.");
                    }
                    else
                    {
                        var ingredientsString = await _mediator.Send(new GetCraftingIngredientsStringQuery(
                            crafting.Ingredients, amount));

                        await _mediator.Send(new CheckCraftingIngredientsInUserCommand(
                            user.Id, crafting.Ingredients, amount));
                        await _mediator.Send(new RemoveCraftingIngredientsFromUserCommand(
                            user.Id, crafting.Ingredients, amount));
                        await _mediator.Send(new RemoveCurrencyFromUserCommand(
                            user.Id, CurrencyType.Ien, craftingPrice));
                        await _mediator.Send(new RemoveEnergyFromUserCommand(
                            user.Id, energyCost * amount));
                        await _mediator.Send(new AddCraftingToUserCommand(
                            user.Id, crafting.Id, amount));
                        await _mediator.Send(new AddCollectionToUserCommand(
                            user.Id, CollectionType.Crafting, crafting.Id));
                        await _mediator.Send(new AddStatisticToUserCommand(
                            user.Id, StatisticType.MakingCrafting, amount));
                        await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                        {
                            AchievementType.FirstCraftResource,
                            AchievementType.Craft30Resource,
                            AchievementType.Craft250Resource,
                            AchievementType.CompleteCollectionCrafting
                        }));

                        embed
                            .WithDescription(
                                $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                                "собрав все необходимые ингредиенты и сверившись с чертежом, " +
                                $"ты успешно изготовил {emotes.GetEmote(crafting.Name)} {amount} " +
                                $"{_local.Localize(LocalizationCategoryType.Crafting, crafting.Name, amount)}." +
                                $"\n{StringExtensions.EmptyChar}")
                            .AddField("Затраченные ингредиенты",
                                ingredientsString +
                                $", {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                                $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}")
                            .AddField("Расход энергии",
                                $"{emotes.GetEmote("Energy")} {energyCost * amount} " +
                                $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost * amount)}");
                    }

                    break;
                }

                case "алкоголь":
                {
                    user.Location.CheckRequiredLocation(LocationType.Village);

                    var alcoholLocal = await _mediator.Send(new GetLocalizationByLocalizedNameQuery(
                        LocalizationCategoryType.Alcohol, name));
                    var alcohol = await _mediator.Send(new GetAlcoholByNameQuery(alcoholLocal.Name));
                    var costPrice = await _mediator.Send(new GetAlcoholCostPriceQuery(alcohol.Ingredients));
                    var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice, amount));

                    if (userCurrency.Amount < craftingPrice)
                    {
                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 5)} " +
                            "для оплаты изготовления.");
                    }
                    else
                    {
                        var ingredientsString = await _mediator.Send(new GetAlcoholIngredientsStringQuery(
                            alcohol.Ingredients, amount));

                        await _mediator.Send(new CheckAlcoholIngredientsInUserCommand(
                            user.Id, alcohol.Ingredients, amount));
                        await _mediator.Send(new RemoveAlcoholIngredientsFromUserCommand(
                            user.Id, alcohol.Ingredients, amount));
                        await _mediator.Send(new RemoveCurrencyFromUserCommand(
                            user.Id, CurrencyType.Ien, craftingPrice));
                        await _mediator.Send(new RemoveEnergyFromUserCommand(
                            user.Id, energyCost * amount));
                        await _mediator.Send(new AddAlcoholToUserCommand(
                            user.Id, alcohol.Id, amount));
                        await _mediator.Send(new AddCollectionToUserCommand(
                            user.Id, CollectionType.Alcohol, alcohol.Id));
                        await _mediator.Send(new AddStatisticToUserCommand(
                            user.Id, StatisticType.MakingAlcohol, amount));
                        await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                        {
                            AchievementType.FirstCraftAlcohol,
                            AchievementType.Craft10Alcohol,
                            AchievementType.Craft80Alcohol,
                            AchievementType.CompleteCollectionAlcohol
                        }));

                        embed
                            .WithDescription(
                                $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                                "собрав все необходимые ингредиенты и сверившись с рецептом, " +
                                $"ты успешно изготовил {emotes.GetEmote(alcohol.Name)} {amount} " +
                                $"{_local.Localize(LocalizationCategoryType.Alcohol, alcohol.Name, amount)}." +
                                $"\n{StringExtensions.EmptyChar}")
                            .AddField("Затраченные ингредиенты",
                                ingredientsString +
                                $", {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                                $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}")
                            .AddField("Расход энергии",
                                $"{emotes.GetEmote("Energy")} {energyCost * amount} " +
                                $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost * amount)}");
                    }

                    break;
                }

                case "напиток":
                {
                    user.Location.CheckRequiredLocation(LocationType.Garden);

                    var drinkLocal = await _mediator.Send(new GetLocalizationByLocalizedNameQuery(
                        LocalizationCategoryType.Drink, name));
                    var drink = await _mediator.Send(new GetDrinkByNameQuery(drinkLocal.Name));
                    var costPrice = await _mediator.Send(new GetDrinkCostPriceQuery(drink.Ingredients));
                    var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice, amount));

                    if (userCurrency.Amount < craftingPrice)
                    {
                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 5)} " +
                            "для оплаты изготовления.");
                    }
                    else
                    {
                        var ingredientsString = await _mediator.Send(new GetDrinkIngredientsStringQuery(
                            drink.Ingredients, amount));

                        await _mediator.Send(new CheckDrinkIngredientsInUserCommand(
                            user.Id, drink.Ingredients, amount));
                        await _mediator.Send(new RemoveDrinkIngredientsFromUserCommand(
                            user.Id, drink.Ingredients, amount));
                        await _mediator.Send(new RemoveCurrencyFromUserCommand(
                            user.Id, CurrencyType.Ien, craftingPrice));
                        await _mediator.Send(new RemoveEnergyFromUserCommand(
                            user.Id, energyCost * amount));
                        await _mediator.Send(new AddDrinkToUserCommand(
                            user.Id, drink.Id, amount));
                        await _mediator.Send(new AddCollectionToUserCommand(
                            user.Id, CollectionType.Drink, drink.Id));
                        await _mediator.Send(new AddStatisticToUserCommand(
                            user.Id, StatisticType.MakingDrink, amount));
                        await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                        {
                            AchievementType.FirstCraftDrink,
                            AchievementType.CompleteCollectionDrink
                        }));

                        embed
                            .WithDescription(
                                $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                                "собрав все необходимые ингредиенты и сверившись с рецептом, " +
                                $"ты успешно изготовил {emotes.GetEmote(drink.Name)} {amount} " +
                                $"{_local.Localize(LocalizationCategoryType.Drink, drink.Name, amount)}." +
                                $"\n{StringExtensions.EmptyChar}")
                            .AddField("Затраченные ингредиенты",
                                ingredientsString +
                                $", {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                                $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}")
                            .AddField("Расход энергии",
                                $"{emotes.GetEmote("Energy")} {energyCost * amount} " +
                                $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost * amount)}");
                    }

                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
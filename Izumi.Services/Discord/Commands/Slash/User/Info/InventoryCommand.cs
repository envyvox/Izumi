using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Models;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Alcohol.Models;
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Box.Models;
using Izumi.Services.Game.Box.Queries;
using Izumi.Services.Game.Crafting.Models;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Crop.Models;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Currency.Models;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Drink.Models;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Fish.Models;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Food.Models;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Models;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Product.Models;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.Seafood.Models;
using Izumi.Services.Game.Seafood.Queries;
using Izumi.Services.Game.Seed.Models;
using Izumi.Services.Game.Seed.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info
{
    public record InventoryCommand(SocketSlashCommand Command) : IRequest;

    public class InventoryHandler : IRequestHandler<InventoryCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteDto> _emotes;

        public InventoryHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(InventoryCommand request, CancellationToken ct)
        {
            var type = request.Command.Data.Options?.First().Value;
            _emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Инвентарь")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Inventory)));

            var desc = string.Empty;

            if (type is null)
            {
                desc = "все полученные предметы попадают сюда:";
                var userCurrencies = await _mediator.Send(new GetUserCurrenciesQuery(user.Id));
                var userBoxes = await _mediator.Send(new GetUserBoxesQuery(user.Id));
                var userGatherings = await _mediator.Send(new GetUserGatheringsQuery(user.Id));
                var userProducts = await _mediator.Send(new GetUserProductsQuery(user.Id));
                var userCraftings = await _mediator.Send(new GetUserCraftingsQuery(user.Id));
                var userAlcohols = await _mediator.Send(new GetUserAlcoholsQuery(user.Id));
                var userDrinks = await _mediator.Send(new GetUserDrinksQuery(user.Id));
                var userSeeds = await _mediator.Send(new GetUserSeedsQuery(user.Id));
                var userCrops = await _mediator.Send(new GetUserCropsQuery(user.Id));
                var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));
                var userFoods = await _mediator.Send(new GetUserFoodsQuery(user.Id));
                var userSeafoods = await _mediator.Send(new GetUserSeafoodsQuery(user.Id));

                embed
                    .AddField(InventoryCategoryType.Currency.Localize(), DisplayCurrency(userCurrencies))
                    .AddField(InventoryCategoryType.Box.Localize(), DisplayBoxes(userBoxes));

                if (userGatherings.Any())
                    embed.AddField(InventoryCategoryType.Gathering.Localize(), DisplayGatherings(userGatherings));

                if (userProducts.Any())
                    embed.AddField(InventoryCategoryType.Product.Localize(), DisplayProducts(userProducts));

                if (userCraftings.Any())
                    embed.AddField(InventoryCategoryType.Crafting.Localize(), DisplayCraftings(userCraftings));

                if (userAlcohols.Any())
                    embed.AddField(InventoryCategoryType.Alcohol.Localize(), DisplayAlcohols(userAlcohols));

                if (userDrinks.Any())
                    embed.AddField(InventoryCategoryType.Drink.Localize(), DisplayDrinks(userDrinks));

                if (userSeeds.Any())
                    embed.AddField(InventoryCategoryType.Seed.Localize(), DisplaySeeds(userSeeds));

                if (userCrops.Any())
                    embed.AddField(InventoryCategoryType.Crop.Localize(), DisplayCrops(userCrops));

                if (userFishes.Any())
                    embed.AddField(InventoryCategoryType.Fish.Localize(), DisplayFish(userFishes));

                if (userFoods.Any())
                    embed.AddField(InventoryCategoryType.Food.Localize(), DisplayFoods(userFoods));

                if (userSeafoods.Any())
                    embed.AddField(InventoryCategoryType.Seafood.Localize(), DisplaySeafoods(userSeafoods));
            }
            else
            {
                switch (type)
                {
                    case "рыба":

                        desc = "тут отображается твоя рыба:";
                        var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));

                        foreach (var rarity in Enum.GetValues(typeof(FishRarityType)).Cast<FishRarityType>())
                        {
                            embed.AddField(rarity.Localize(),
                                DisplayFish(userFishes.Where(x => x.Fish.Rarity == rarity)));
                        }

                        break;
                    case "семена":

                        desc = "тут отображается твои семена:";
                        var userSeeds = await _mediator.Send(new GetUserSeedsQuery(user.Id));

                        foreach (var season in Enum
                            .GetValues(typeof(SeasonType))
                            .Cast<SeasonType>()
                            .Where(x => x != SeasonType.Any))
                        {
                            embed.AddField(season.Localize(),
                                DisplaySeeds(userSeeds.Where(x => x.Seed.Season == season)));
                        }

                        break;
                    case "урожай":

                        desc = "тут отображается твой урожай:";
                        var userCrops = await _mediator.Send(new GetUserCropsQuery(user.Id));

                        foreach (var season in Enum
                            .GetValues(typeof(SeasonType))
                            .Cast<SeasonType>()
                            .Where(x => x != SeasonType.Any))
                        {
                            embed.AddField(season.Localize(),
                                DisplayCrops(userCrops.Where(x => x.Crop.Seed.Season == season)));
                        }

                        break;

                    case "блюда":

                        desc = "тут отображаются твои блюда:";
                        var userFoods = await _mediator.Send(new GetUserFoodsQuery(user.Id));

                        foreach (var category in Enum.GetValues(typeof(FoodCategoryType)).Cast<FoodCategoryType>())
                        {
                            embed.AddField(category.Localize(),
                                DisplayFoods(userFoods.Where(x => x.Food.Category == category)));
                        }

                        break;
                }
            }

            embed.WithDescription(
                $"{_emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                $"{desc}\n{_emotes.GetEmote("Blank")}");

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }

        private string DisplayCurrency(IReadOnlyDictionary<CurrencyType, UserCurrencyDto> userCurrencies)
        {
            var str = Enum
                .GetValues(typeof(CurrencyType))
                .Cast<CurrencyType>()
                .Aggregate(string.Empty, (s, v) =>
                    s +
                    $"{_emotes.GetEmote(v.ToString())} {(userCurrencies.ContainsKey(v) ? userCurrencies[v].Amount : 0)} {_local.Localize(LocalizationCategoryType.Currency, v.ToString(), userCurrencies.ContainsKey(v) ? userCurrencies[v].Amount : 0)}, ");

            return str.RemoveFromEnd(2);
        }

        private string DisplayBoxes(IReadOnlyDictionary<BoxType, UserBoxDto> userBoxes)
        {
            var str = Enum
                .GetValues(typeof(BoxType))
                .Cast<BoxType>()
                .Aggregate(string.Empty, (s, v) =>
                    s +
                    $"{_emotes.GetEmote(v.EmoteName())} {(userBoxes.ContainsKey(v) ? userBoxes[v].Amount : 0)} {_local.Localize(LocalizationCategoryType.Box, v.ToString(), userBoxes.ContainsKey(v) ? userBoxes[v].Amount : 0)}, ");

            return str.RemoveFromEnd(2) + $"\n\n{_emotes.GetEmote("Arrow")} Напиши `/открыть [количество] [название]` чтобы открыть коробку.";
        }

        private string DisplayGatherings(IEnumerable<UserGatheringDto> userGatherings)
        {
            var str = userGatherings.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Gathering.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Gathering, v.Gathering.Name, v.Amount)}, ");

            return str.Length > 0 ? str.RemoveFromEnd(2) : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplayCraftings(IEnumerable<UserCraftingDto> userCraftings)
        {
            var str = userCraftings.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Crafting.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Crafting, v.Crafting.Name, v.Amount)}, ");

            return str.Length > 0 ? str.RemoveFromEnd(2) : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplayAlcohols(IEnumerable<UserAlcoholDto> userAlcohols)
        {
            var str = userAlcohols.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Alcohol.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Alcohol, v.Alcohol.Name, v.Amount)}, ");

            return str.Length > 0 ? str.RemoveFromEnd(2) : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplayDrinks(IEnumerable<UserDrinkDto> userDrinks)
        {
            var str = userDrinks.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Drink.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Drink, v.Drink.Name, v.Amount)}, ");

            return str.Length > 0 ? str.RemoveFromEnd(2) : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplayProducts(IEnumerable<UserProductDto> userProducts)
        {
            var str = userProducts.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Product.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Product, v.Product.Name, v.Amount)}, ");

            return str.Length > 0 ? str.RemoveFromEnd(2) : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplaySeeds(IEnumerable<UserSeedDto> userSeeds)
        {
            var str = userSeeds.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Seed.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Seed, v.Seed.Name, v.Amount)}, ");

            return str.Length > 0
                ? str.Length > 1024
                    ? "У тебя слишком много семян, напиши `/инвентарь семена` чтобы посмотреть их"
                    : str.RemoveFromEnd(2)
                : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplayCrops(IEnumerable<UserCropDto> userCrops)
        {
            var str = userCrops.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Crop.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Crop, v.Crop.Name, v.Amount)}, ");

            return str.Length > 0
                ? str.Length > 1024
                    ? "У тебя слишком много урожая, напиши `/инвентарь урожай` чтобы посмотреть его"
                    : str.RemoveFromEnd(2)
                : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplayFish(IEnumerable<UserFishDto> userFishes)
        {
            var str = userFishes.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Fish.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Fish, v.Fish.Name, v.Amount)}, ");

            return str.Length > 0
                ? str.Length > 1024
                    ? "У тебя слишком много рыбы, напиши `/инвентарь рыба` чтобы посмотреть ее"
                    : str.RemoveFromEnd(2)
                : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplayFoods(IEnumerable<UserFoodDto> userFoods)
        {
            var str = userFoods.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Food.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Food, v.Food.Name, v.Amount)}, ");

            return str.Length > 0
                ? str.Length > 1024
                    ? "У тебя слишком много блюд, напиши `/инвентарь блюда` чтобы посмотреть их"
                    : str.RemoveFromEnd(2)
                : "У тебя нет ни одного предмета этого типа";
        }

        private string DisplaySeafoods(IEnumerable<UserSeafoodDto> userSeafoods)
        {
            var str = userSeafoods.Aggregate(string.Empty, (s, v) =>
                s +
                $"{_emotes.GetEmote(v.Seafood.Name)} {v.Amount} {_local.Localize(LocalizationCategoryType.Seafood, v.Seafood.Name, v.Amount)}, ");

            return str.Length > 0 ? str.RemoveFromEnd(2) : "У тебя нет ни одного предмета этого типа";
        }
    }
}

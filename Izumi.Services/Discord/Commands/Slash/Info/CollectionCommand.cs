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
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Collection.Queries;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Food.Models;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Info
{
    public record CollectionCommand(SocketSlashCommand Command) : IRequest;

    public class CollectionHandler : IRequestHandler<CollectionCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CollectionHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CollectionCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var type = (CollectionType) (long) request.Command.Data.Options.First().Value;
            var userCollections = await _mediator.Send(new GetUserCollectionsQuery(user.Id, type));

            var embed = new EmbedBuilder()
                .WithAuthor("Коллекция")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"тут отображается твоя коллекция в категории **{type.Localize()}**:" +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Collection)));

            switch (type)
            {
                case CollectionType.Gathering:

                    var gatherings = await _mediator.Send(new GetGatheringsQuery());
                    var gatheringString = string.Empty;

                    foreach (var gathering in gatherings)
                    {
                        var exist = userCollections.Any(x => x.ItemId == gathering.Id);

                        gatheringString +=
                            $"{emotes.GetEmote(gathering.Name + (exist ? "" : "BW"))} " +
                            $"{_local.Localize(LocalizationCategoryType.Gathering, gathering.Name)} ";
                    }

                    embed.AddField(type.Localize(), gatheringString);

                    break;
                case CollectionType.Crafting:

                    var craftings = await _mediator.Send(new GetCraftingsQuery());
                    var craftingString = string.Empty;

                    foreach (var crafting in craftings)
                    {
                        var exist = userCollections.Any(x => x.ItemId == crafting.Id);

                        craftingString +=
                            $"{emotes.GetEmote(crafting.Name + (exist ? "" : "BW"))} " +
                            $"{_local.Localize(LocalizationCategoryType.Crafting, crafting.Name)} ";
                    }

                    embed.AddField(type.Localize(), craftingString);

                    break;
                case CollectionType.Alcohol:

                    var alcohols = await _mediator.Send(new GetAlcoholsQuery());
                    var alcoholString = string.Empty;

                    foreach (var alcohol in alcohols)
                    {
                        var exist = userCollections.Any(x => x.ItemId == alcohol.Id);

                        alcoholString +=
                            $"{emotes.GetEmote(alcohol.Name + (exist ? "" : "BW"))} " +
                            $"{_local.Localize(LocalizationCategoryType.Alcohol, alcohol.Name)} ";
                    }

                    embed.AddField(type.Localize(), alcoholString);

                    break;
                case CollectionType.Drink:

                    var drinks = await _mediator.Send(new GetDrinksQuery());
                    var drinkString = string.Empty;

                    foreach (var drink in drinks)
                    {
                        var exist = userCollections.Any(x => x.ItemId == drink.Id);

                        drinkString +=
                            $"{emotes.GetEmote(drink.Name + (exist ? "" : "BW"))} " +
                            $"{_local.Localize(LocalizationCategoryType.Drink, drink.Name)} ";
                    }

                    embed.AddField(type.Localize(), drinkString);

                    break;
                case CollectionType.Crop:

                    var crops = await _mediator.Send(new GetCropsQuery());
                    var springCropString = string.Empty;
                    var summerCropString = string.Empty;
                    var autumnCropString = string.Empty;

                    foreach (var crop in crops)
                    {
                        var exist = userCollections.Any(x => x.ItemId == crop.Id);
                        var displayString =
                            $"{emotes.GetEmote(crop.Name + (exist ? "" : "BW"))} " +
                            $"{_local.Localize(LocalizationCategoryType.Crop, crop.Name)} ";

                        switch (crop.Seed.Season)
                        {
                            case SeasonType.Spring:
                                springCropString += displayString;
                                break;
                            case SeasonType.Summer:
                                summerCropString += displayString;
                                break;
                            case SeasonType.Autumn:
                                autumnCropString += displayString;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    embed
                        .AddField("Весенний урожай", springCropString)
                        .AddField("Летний урожай", summerCropString)
                        .AddField("Осенний урожай", autumnCropString);

                    break;
                case CollectionType.Fish:

                    var fishes = await _mediator.Send(new GetFishesQuery());
                    var commonFishString = string.Empty;
                    var rareFishString = string.Empty;
                    var epicFishString = string.Empty;
                    var mythicalFishString = string.Empty;
                    var legendaryFishString = string.Empty;

                    foreach (var fish in fishes)
                    {
                        var exist = userCollections.Any(x => x.ItemId == fish.Id);
                        var displayString =
                            $"{emotes.GetEmote(fish.Name + (exist ? "" : "BW"))} " +
                            $"{_local.Localize(LocalizationCategoryType.Fish, fish.Name)} ";

                        switch (fish.Rarity)
                        {
                            case FishRarityType.Common:
                                commonFishString += displayString;
                                break;
                            case FishRarityType.Rare:
                                rareFishString += displayString;
                                break;
                            case FishRarityType.Epic:
                                epicFishString += displayString;
                                break;
                            case FishRarityType.Mythical:
                                mythicalFishString += displayString;
                                break;
                            case FishRarityType.Legendary:
                                legendaryFishString += displayString;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    embed
                        .AddField(FishRarityType.Common.Localize(), commonFishString)
                        .AddField(FishRarityType.Rare.Localize(), rareFishString)
                        .AddField(FishRarityType.Epic.Localize(), epicFishString)
                        .AddField(FishRarityType.Mythical.Localize(), mythicalFishString)
                        .AddField(FishRarityType.Legendary.Localize(), legendaryFishString);

                    break;
                case CollectionType.Food:

                    var foods = await _mediator.Send(new GetFoodsQuery());

                    while (foods.Count > 10)
                    {
                        var displayFoods = foods
                            .Take(10)
                            .ToList();

                        foods.RemoveRange(0, 10);

                        var foodString = string.Empty;

                        foreach (var food in displayFoods)
                        {
                            var exist = userCollections.Any(x => x.ItemId == food.Id);
                            foodString +=
                                $"{emotes.GetEmote(food.Name + (exist ? "" : "BW"))} " +
                                $"{_local.Localize(LocalizationCategoryType.Food, food.Name)} ";
                        }

                        embed.AddField(StringExtensions.EmptyChar, foodString);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
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
using Izumi.Services.Extensions;
using Izumi.Services.Game.Box.Commands;
using Izumi.Services.Game.Box.Queries;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Crop.Commands;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Fish.Commands;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Gathering.Commands;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Product.Commands;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash
{
    public record OpenBoxCommand(SocketSlashCommand Command) : IRequest;

    public class OpenBoxHandler : IRequestHandler<OpenBoxCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        private Dictionary<string, EmoteDto> _emotes;
        private readonly Random _random = new();

        public OpenBoxHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(OpenBoxCommand request, CancellationToken ct)
        {
            _emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var boxAmount = (uint) (long) request.Command.Data.Options.First().Value;
            var boxType = (BoxType) (long) request.Command.Data.Options.Last().Value;
            var userBox = await _mediator.Send(new GetUserBoxQuery(user.Id, boxType));

            var embed = new EmbedBuilder()
                .WithAuthor("Открытие коробок");

            if (userBox.Amount < boxAmount)
            {
                embed.WithDescription(
                    $"{_emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя нет столько {_emotes.GetEmote(boxType.EmoteName())} " +
                    $"{_local.Localize(LocalizationCategoryType.Box, boxType.ToString())} сколько ты хочешь открыть.");
            }
            else
            {
                await _mediator.Send(new RemoveBoxFromUserCommand(user.Id, boxType, boxAmount));

                var reward = boxType switch
                {
                    BoxType.Capital => await OpenCapitalBox(user.Id, boxAmount),
                    BoxType.Garden => await OpenGardenBox(user.Id, boxAmount),
                    BoxType.Seaport => await OpenSeaportBox(user.Id, boxAmount),
                    BoxType.Castle => await OpenCastleBox(user.Id, boxAmount),
                    BoxType.Village => await OpenVillageBox(user.Id, boxAmount),
                    _ => throw new ArgumentOutOfRangeException()
                };

                embed
                    .WithDescription(
                        $"{_emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно открыл {_emotes.GetEmote(boxType.EmoteName())} {boxAmount} " +
                        $"{_local.Localize(LocalizationCategoryType.Box, boxType.ToString(), boxAmount)} и вот что оказалось внутри:" +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField("Содержимое",
                        reward.Length <= 1024
                            ? reward
                            : "Ты открыл так много коробок, что ни один писарь не соглалился составлять полный список. " +
                              "Однако не стоит переживать, все предметы доставлены в твой инвентарь.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }

        private async Task<string> OpenCapitalBox(long userId, uint boxAmount)
        {
            var minAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxCapitalMinAmount));
            var maxAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxCapitalMaxAmount));

            uint total = 0;
            for (var i = 0; i < boxAmount; i++) total += (uint) _random.Next(minAmount, maxAmount);

            await _mediator.Send(new AddCurrencyToUserCommand(userId, CurrencyType.Ien, total));

            return
                $"{_emotes.GetEmote(CurrencyType.Ien.ToString())} {total} " +
                $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), total)}";
        }

        private async Task<string> OpenGardenBox(long userId, uint boxAmount)
        {
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(
                LocationType.ExploreGarden));

            var str = string.Empty;
            foreach (var gathering in gatherings)
            {
                uint total = 0;
                for (var i = 0; i < boxAmount; i++)
                {
                    var chance = await _mediator.Send(new GetGatheringPropertyValueQuery(
                        gathering.Id, GatheringPropertyType.GatheringChance));
                    var doubleChance = await _mediator.Send(new GetGatheringPropertyValueQuery(
                        gathering.Id, GatheringPropertyType.GatheringDoubleChance));
                    var amount = await _mediator.Send(new GetGatheringPropertyValueQuery(
                        gathering.Id, GatheringPropertyType.GatheringAmount));
                    var successAmount = await _mediator.Send(new GetSuccessAmountQuery(chance, doubleChance, amount));

                    if (successAmount < 1) continue;

                    total += successAmount;
                }

                if (total < 1) continue;

                await _mediator.Send(new AddGatheringToUserCommand(userId, gathering.Id, total));

                str +=
                    $"{_emotes.GetEmote(gathering.Name)} {total} " +
                    $"{_local.Localize(LocalizationCategoryType.Gathering, gathering.Name, total)}, ";
            }

            return str.RemoveFromEnd(2);
        }

        private async Task<string> OpenSeaportBox(long userId, uint boxAmount)
        {
            var minAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxSeaportMinAmount));
            var maxAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxSeaportMaxAmount));
            var rarity = (FishRarityType) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxSeaportRarity));

            var str = string.Empty;
            var fishes = new Dictionary<Guid, Dictionary<string, uint>>();
            for (var i = 0; i < boxAmount; i++)
            {
                var randomFish = await _mediator.Send(new GetRandomFishWithRarityQuery(rarity));
                var randomAmount = (uint) _random.Next(minAmount, maxAmount);

                if (fishes.ContainsKey(randomFish.Id)) fishes[randomFish.Id][randomFish.Name] += randomAmount;
                else fishes.Add(randomFish.Id, new Dictionary<string, uint> { { randomFish.Name, randomAmount } });
            }

            foreach (var (fishId, fish) in fishes)
            {
                await _mediator.Send(new AddFishToUserCommand(userId, fishId, fish.Values.First()));

                str +=
                    $"{_emotes.GetEmote(fish.Keys.First())} {fish.Values.First()} " +
                    $"{_local.Localize(LocalizationCategoryType.Fish, fish.Keys.First(), fish.Values.First())}, ";
            }

            return str.RemoveFromEnd(2);
        }

        private async Task<string> OpenCastleBox(long userId, uint boxAmount)
        {
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(
                LocationType.ExploreCastle));

            var str = string.Empty;
            foreach (var gathering in gatherings)
            {
                uint total = 0;
                for (var i = 0; i < boxAmount; i++)
                {
                    var chance = await _mediator.Send(new GetGatheringPropertyValueQuery(
                        gathering.Id, GatheringPropertyType.GatheringChance));
                    var doubleChance = await _mediator.Send(new GetGatheringPropertyValueQuery(
                        gathering.Id, GatheringPropertyType.GatheringDoubleChance));
                    var amount = await _mediator.Send(new GetGatheringPropertyValueQuery(
                        gathering.Id, GatheringPropertyType.GatheringAmount));
                    var successAmount = await _mediator.Send(new GetSuccessAmountQuery(chance, doubleChance, amount));

                    if (successAmount < 1) continue;

                    total += successAmount;
                }

                if (total < 1) continue;

                await _mediator.Send(new AddGatheringToUserCommand(userId, gathering.Id, total));

                str +=
                    $"{_emotes.GetEmote(gathering.Name)} {total} " +
                    $"{_local.Localize(LocalizationCategoryType.Gathering, gathering.Name, total)}, ";
            }

            return str.RemoveFromEnd(2);
        }

        private async Task<string> OpenVillageBox(long userId, uint boxAmount)
        {
            var productMinAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxVillageProductMinAmount));
            var productMaxAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxVillageProductMaxAmount));
            var cropMinAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxVillageCropMinAmount));
            var cropMaxAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.OpenBoxVillageCropMaxAmount));
            var products = await _mediator.Send(new GetProductsQuery());

            var str = string.Empty;
            foreach (var product in products)
            {
                uint total = 0;
                for (var i = 0; i < boxAmount; i++) total += (uint) _random.Next(productMinAmount, productMaxAmount);

                if (total < 1) continue;

                await _mediator.Send(new AddProductToUserCommand(userId, product.Id, total));

                str +=
                    $"{_emotes.GetEmote(product.Name)} {total} " +
                    $"{_local.Localize(LocalizationCategoryType.Product, product.Name, total)}, ";
            }

            var crops = new Dictionary<Guid, Dictionary<string, uint>>();
            for (int i = 0; i < boxAmount; i++)
            {
                var randomCrop = await _mediator.Send(new GetRandomCropQuery());
                var randomCropAmount = (uint) _random.Next(cropMinAmount, cropMaxAmount);

                if (crops.ContainsKey(randomCrop.Id)) crops[randomCrop.Id][randomCrop.Name] += randomCropAmount;
                else crops.Add(randomCrop.Id, new Dictionary<string, uint> { { randomCrop.Name, randomCropAmount } });
            }

            foreach (var (cropId, crop) in crops)
            {
                await _mediator.Send(new AddCropToUserCommand(userId, cropId, crop.Values.First()));

                str +=
                    $"{_emotes.GetEmote(crop.Keys.First())} {crop.Values.First()} " +
                    $"{_local.Localize(LocalizationCategoryType.Crop, crop.Keys.First(), crop.Values.First())}, ";
            }

            return str.RemoveFromEnd(2);
        }
    }
}

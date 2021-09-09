﻿using System;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Game.Collection.Commands;
using Izumi.Services.Game.Fish.Commands;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.World.Queries;
using Izumi.Services.Hangfire.Commands;
using MediatR;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteFishing
{
    public class CompleteFishingJob : ICompleteFishingJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CompleteFishingJob(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(long userId)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var timesDay = await _mediator.Send(new GetCurrentTimesDayQuery());
            var weather = await _mediator.Send(new GetWeatherTodayQuery());
            var season = await _mediator.Send(new GetCurrentSeasonQuery());
            var rarity = await _mediator.Send(new GetRandomFishRarityQuery());
            var fish = await _mediator.Send(new GetRandomFishWithParamsQuery(rarity, timesDay, weather, season));
            var success = await _mediator.Send(new CheckFishingSuccessQuery(fish.Rarity));

            await _mediator.Send(new UpdateUserLocationCommand(userId, LocationType.Seaport));
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireJobType.Explore));

            var embed = new EmbedBuilder()
                .WithAuthor(LocationType.Fishing.Localize())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Fishing)));

            if (success)
            {
                await _mediator.Send(new AddFishToUserCommand(userId, fish.Id, 1));
                await _mediator.Send(new AddCollectionToUserCommand(userId, CollectionType.Fish, fish.Id));
                // todo check achievement

                switch (rarity)
                {
                    case FishRarityType.Common:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.FishingCommon));
                        break;
                    case FishRarityType.Rare:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.FishingRare));
                        break;
                    case FishRarityType.Epic:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.FishingEpic));
                        // todo check achievement
                        break;
                    case FishRarityType.Mythical:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.FishingMythical));
                        // todo check achievement
                        break;
                    case FishRarityType.Legendary:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.FishingLegendary));
                        // todo check achievement
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                embed.WithDescription(
                    "Ты возвращаешься с улыбкой на лице и гордо демонстрируешь жителям города " +
                    $"{emotes.GetEmote(fish.Name)} {_local.Localize(LocalizationCategoryType.Fish, fish.Name)}." +
                    "\nЕсть чем гордиться, понимаю, но рыбы в здешних водах еще полно, возвращайся за новым уловом поскорее!");
            }
            else
            {
                embed.WithDescription(
                    "Сегодня явно не твой день, ведь вернувшись тебе совсем нечем похвастаться перед жителями города." +
                    $"\nТы почти поймал {emotes.GetEmote(fish.Name)} {_local.Localize(LocalizationCategoryType.Fish, fish.Name)}, " +
                    "однако хитрая рыба смогла сорваться с крючка. Но не расстраивайся, " +
                    "рыба в здешних водах никуда не денется, возвращайся и попытай удачу еще раз!");
            }

            await _mediator.Send(new SendEmbedToUserCommand((ulong) userId, embed));
        }
    }
}
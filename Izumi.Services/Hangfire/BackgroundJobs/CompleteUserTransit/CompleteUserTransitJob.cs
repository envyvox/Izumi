using System;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Hangfire.Commands;
using MediatR;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteUserTransit
{
    public class CompleteUserTransitJob : ICompleteUserTransitJob
    {
        private readonly IMediator _mediator;

        public CompleteUserTransitJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(long userId, LocationType destination)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var channels = await _mediator.Send(new GetChannelsQuery());
            DiscordChannelType descChannel;
            DiscordChannelType whatToDoChannel;
            DiscordChannelType eventsChannel;

            switch (destination)
            {
                case LocationType.Capital:

                    descChannel = DiscordChannelType.CapitalDesc;
                    whatToDoChannel = DiscordChannelType.CapitalWhatToDo;
                    eventsChannel = DiscordChannelType.CapitalEvents;

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToCapital));

                    break;
                case LocationType.Garden:

                    descChannel = DiscordChannelType.GardenDesc;
                    whatToDoChannel = DiscordChannelType.GardenWhatToDo;
                    eventsChannel = DiscordChannelType.GardenEvents;

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToGarden));

                    break;
                case LocationType.Seaport:

                    descChannel = DiscordChannelType.SeaportDesc;
                    whatToDoChannel = DiscordChannelType.SeaportWhatToDo;
                    eventsChannel = DiscordChannelType.SeaportEvents;

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToSeaport));

                    break;
                case LocationType.Castle:

                    descChannel = DiscordChannelType.CastleDesc;
                    whatToDoChannel = DiscordChannelType.CastleWhatToDo;
                    eventsChannel = DiscordChannelType.CastleEvents;

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToCastle));

                    break;
                case LocationType.Village:

                    descChannel = DiscordChannelType.VillageDesc;
                    whatToDoChannel = DiscordChannelType.VillageWhatToDo;
                    eventsChannel = DiscordChannelType.VillageEvents;

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToVillage));

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(destination), destination, null);
            }

            await _mediator.Send(new UpdateUserLocationCommand(userId, destination));
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireJobType.Transit));
            await _mediator.Send(new RemoveRoleFromGuildUserCommand((ulong) userId, DiscordRoleType.LocationInTransit));
            await _mediator.Send(new AddRoleToGuildUserCommand((ulong) userId, destination.Role()));
            await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.Transit));
            await _mediator.Send(new CheckAchievementInUserCommand(userId, AchievementType.FirstTransit));

            var embed = new EmbedBuilder()
                .WithAuthor("Прибытие в локацию")
                .WithDescription(
                    $"Ты достиг точки прибытия, добро пожаловать в **{destination.Localize()}**!" +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Каналы локации",
                    $"<#{channels[descChannel].Id}>, <#{channels[whatToDoChannel].Id}>, <#{channels[eventsChannel].Id}>")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.InTransit)));

            await _mediator.Send(new SendEmbedToUserCommand((ulong) userId, embed));
        }
    }
}

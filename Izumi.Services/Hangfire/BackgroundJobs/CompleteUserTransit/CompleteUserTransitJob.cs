using System;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteUserTransit
{
    public class CompleteUserTransitJob : ICompleteUserTransitJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CompleteUserTransitJob> _logger;

        public CompleteUserTransitJob(
            IMediator mediator,
            ILogger<CompleteUserTransitJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(long userId, LocationType destination)
        {
            _logger.LogInformation(
                "Complete user transit job executed for user {UserId} and destination {Destination}",
                userId, destination.ToString());

            switch (destination)
            {
                case LocationType.Capital:

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToCapital));

                    break;
                case LocationType.Garden:

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToGarden));

                    break;
                case LocationType.Seaport:

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToSeaport));

                    break;
                case LocationType.Castle:

                    await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.TransitToCastle));

                    break;
                case LocationType.Village:

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
                    $"Ты достиг точки прибытия, добро пожаловать в **{destination.Localize()}**!")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.InTransit)));

            await _mediator.Send(new SendEmbedToUserCommand((ulong) userId, embed));
        }
    }
}
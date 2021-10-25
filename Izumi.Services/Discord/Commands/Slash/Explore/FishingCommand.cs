using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hangfire;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteFishing;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Explore
{
    public record FishingCommand(SocketSlashCommand Command) : IRequest;

    public class FishingHandler : IRequestHandler<FishingCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FishingHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FishingCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Seaport);

            var emotes = DiscordRepository.Emotes;
            var exploreTime = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.ActionTimeMinutesExplore));
            var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EnergyCostExplore));
            var actionTime = await _mediator.Send(new GetActionTimeQuery(
                TimeSpan.FromMinutes(exploreTime), user.Energy));

            await _mediator.Send(new UpdateUserLocationCommand(user.Id, LocationType.Fishing));
            await _mediator.Send(new CreateUserMovementCommand(
                user.Id, LocationType.Fishing, LocationType.Seaport, DateTimeOffset.UtcNow.Add(actionTime)));
            await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost));

            var jobId = BackgroundJob.Schedule<ICompleteFishingJob>(
                x => x.Execute(user.Id),
                actionTime);

            await _mediator.Send(new CreateUserHangfireJobCommand(
                user.Id, HangfireJobType.Explore, jobId, DateTimeOffset.UtcNow.Add(actionTime)));

            var embed = new EmbedBuilder()
                .WithAuthor(LocationType.Fishing.Localize())
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"**{LocationType.Seaport.Localize()}** полон желающих поймать крутой улов и теперь ты один из них. " +
                    "В надежде что богиня фортуны пошлет тебе улов потяжелее ты отправляешься на рыбалку, " +
                    "но даже самые опытные никогда не могут знать заранее насколько удачно все пройдет." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Ожидаемая награда",
                    $"{emotes.GetEmote("SlimejackBW")} случайная рыба")
                .AddField("Длительность",
                    actionTime.Humanize(2, new CultureInfo("ru-RU")), true)
                .AddField("Расход энергии",
                    $"{emotes.GetEmote("Energy")} {energyCost} " +
                    $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost)}", true)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Fishing)));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

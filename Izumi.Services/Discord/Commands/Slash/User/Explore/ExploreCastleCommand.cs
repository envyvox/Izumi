using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hangfire;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreCastle;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.User.Explore
{
    public record ExploreCastleCommand(SocketSlashCommand Command) : IRequest;

    public class ExploreCastleHandler : IRequestHandler<ExploreCastleCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ExploreCastleHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ExploreCastleCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Castle);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var exploreTime = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.ActionTimeMinutesExplore));
            var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EnergyCostExplore));
            var actionTime = await _mediator.Send(new GetActionTimeQuery(
                TimeSpan.FromMinutes(exploreTime), user.Energy));
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(
                LocationType.ExploreCastle));

            await _mediator.Send(new UpdateUserLocationCommand(user.Id, LocationType.ExploreCastle));
            await _mediator.Send(new CreateUserMovementCommand(
                user.Id, LocationType.ExploreCastle, LocationType.Castle, DateTimeOffset.UtcNow.Add(actionTime)));
            await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost));

            var jobId = BackgroundJob.Schedule<ICompleteExploreCastleJob>(
                x => x.Execute(user.Id),
                actionTime);

            await _mediator.Send(new CreateUserHangfireJobCommand(
                user.Id, HangfireJobType.Explore, jobId, DateTimeOffset.UtcNow.Add(actionTime)));

            var embed = new EmbedBuilder()
                .WithAuthor(LocationType.ExploreCastle.Localize())
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"ты отправился на исследование пещер, которых хватает вокруг **{LocationType.Castle.Localize()}**, " +
                    "нельзя сразу сказать что тебя ожидает впереди." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Ожидаемая награда",
                    gatherings
                        .Aggregate(string.Empty, (s, v) =>
                            s +
                            $"{emotes.GetEmote(v.Name)} {_local.Localize(LocalizationCategoryType.Gathering, v.Name)}, ")
                        .RemoveFromEnd(2))
                .AddField("Длительность",
                    actionTime.Humanize(2, new CultureInfo("ru-RU")), true)
                .AddField("Расход энергии",
                    $"{emotes.GetEmote("Energy")} {energyCost} " +
                    $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost)}", true)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ExploreCastle)));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

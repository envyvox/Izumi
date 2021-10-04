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
using Izumi.Services.Game.Field.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteFieldWatering;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Field
{
    public record FieldWaterCommand(SocketSlashCommand Command) : IRequest;

    public class FieldWaterHandler : IRequestHandler<FieldWaterCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldWaterHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FieldWaterCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Village);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var userFields = await _mediator.Send(new GetUserFieldsQuery(user.Id));
            var fieldsToWater = userFields.Count(x => x.State == FieldStateType.Planted);

            var embed = new EmbedBuilder()
                .WithAuthor("Поливка участка земли")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Field)));

            if (userFields.Count < 1)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя нет {emotes.GetEmote(BuildingType.HarvestField.ToString())} участка земли, для начала " +
                    "напиши `/участок купить` чтобы приобрести его.");
            }
            else if (fieldsToWater < 1)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"на твоем {emotes.GetEmote(BuildingType.HarvestField.ToString())} участке земли нет клеток которые " +
                    "нуждаются в поливке.");
            }
            else
            {
                var wateringTimePerField = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.ActionTimeMinutesFieldWater));
                var energyCostPerField = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.EnergyCostFieldWater));
                var actionTime = await _mediator.Send(new GetActionTimeQuery(
                    TimeSpan.FromMinutes(wateringTimePerField * fieldsToWater), user.Energy));
                var energyCost = energyCostPerField * (uint) fieldsToWater;

                await _mediator.Send(new UpdateUserLocationCommand(user.Id, LocationType.FieldWatering));
                await _mediator.Send(new CreateUserMovementCommand(
                    user.Id, LocationType.FieldWatering, LocationType.Village, DateTimeOffset.UtcNow.Add(actionTime)));
                await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost));

                var jobId = BackgroundJob.Schedule<ICompleteFieldWateringJob>(
                    x => x.Execute(user.Id),
                    actionTime);

                await _mediator.Send(new CreateUserHangfireJobCommand(
                    user.Id, HangfireJobType.FieldWatering, jobId, DateTimeOffset.UtcNow.Add(actionTime)));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты отправился поливать свой {emotes.GetEmote(BuildingType.HarvestField.ToString())} участок земли." +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField("Длительность",
                        $"{actionTime.Humanize(2, new CultureInfo("ru-RU"))}",
                        true)
                    .AddField("Расход энергии",
                        $"{emotes.GetEmote("Energy")} {energyCost} " +
                        $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost)}",
                        true);
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

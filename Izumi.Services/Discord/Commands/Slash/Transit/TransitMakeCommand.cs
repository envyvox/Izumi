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
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.Transit.Queries;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteUserTransit;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Transit
{
    public record TransitMakeCommand(SocketSlashCommand Command) : IRequest;

    public class TransitMakeHandler : IRequestHandler<TransitMakeCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public TransitMakeHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(TransitMakeCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var destination = (LocationType) (long) request.Command.Data.Options.First().Value;
            var hasMovement = await _mediator.Send(new CheckUserHasMovementQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Отправление");

            if (hasMovement)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты находишься в пути и не можешь отправиться в другую локацию до прибытия.");
            }
            else if (destination == user.Location)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты уже находишься в данной локации, не нужно никуда отправляться.");
            }
            else
            {
                var transit = await _mediator.Send(new GetTransitQuery(user.Location, destination));
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

                if (userCurrency.Amount < transit.Price)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"для оплаты этого транспорта необходимо {emotes.GetEmote(CurrencyType.Ien.ToString())} {transit.Price} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), transit.Price)}, которых у тебя нет.");
                }
                else
                {
                    var transitTime = await _mediator.Send(new GetActionTimeQuery(transit.Duration, user.Energy));
                    var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.EnergyCostTransit));

                    await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, transit.Price));
                    await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost));
                    await _mediator.Send(new UpdateUserLocationCommand(user.Id, LocationType.InTransit));
                    await _mediator.Send(new CreateUserMovementCommand(
                        user.Id, user.Location, transit.Destination, DateTimeOffset.UtcNow.Add(transitTime)));
                    await _mediator.Send(new RemoveRoleFromGuildUserCommand(
                        request.Command.User.Id, user.Location.Role()));
                    await _mediator.Send(new AddRoleToGuildUserCommand(
                        request.Command.User.Id, DiscordRoleType.LocationInTransit));

                    var jobId = BackgroundJob.Schedule<ICompleteUserTransitJob>(
                        x => x.Execute(user.Id, transit.Destination),
                        transitTime);

                    await _mediator.Send(new CreateUserHangfireJobCommand(
                        user.Id, HangfireJobType.Transit, jobId, DateTimeOffset.UtcNow.Add(transitTime)));

                    embed
                        .WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"ты отправляешься в **{transit.Destination.Localize()}**, хорошей дороги!" +
                            $"\n{StringExtensions.EmptyChar}")
                        .AddField("Стоимость перемещения",
                            $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {transit.Price} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), transit.Price)}",
                            true)
                        .AddField("Длительность",
                            $"{transitTime.Humanize(2, new CultureInfo("ru-RU"))}",
                            true)
                        .AddField("Расход энергии",
                            $"{emotes.GetEmote("Energy")} {energyCost} " +
                            $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost)}",
                            true)
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.InTransit)));
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

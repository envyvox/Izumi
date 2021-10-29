using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Reputation.Queries;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Info
{
    public record ReputationsCommand(SocketSlashCommand Command) : IRequest;

    public class ReputationsHandler : IRequestHandler<ReputationsCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ReputationsHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ReputationsCommand request, CancellationToken ct)
        {
            var type = (ReputationType) (long) request.Command.Data.Options.First().Value;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userReputation = await _mediator.Send(new GetUserReputationQuery(user.Id, type));

            var boxAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.ReputationBoxAmount));
            var rep1000PearlAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.Reputation1000PearlAmount));
            var rep5000PearlAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.Reputation5000PearlAmount));

            BoxType boxType;
            long foodIncId;
            TitleType title;

            switch (type)
            {
                case ReputationType.Capital:

                    boxType = BoxType.Capital;
                    foodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationCapitalFoodId));
                    title = (TitleType) await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationCapitalTitleNumber));

                    break;
                case ReputationType.Garden:

                    boxType = BoxType.Garden;
                    foodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationGardenFoodId));
                    title = (TitleType) await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationGardenTitleNumber));

                    break;
                case ReputationType.Seaport:

                    boxType = BoxType.Seaport;
                    foodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationSeaportFoodId));
                    title = (TitleType) await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationSeaportTitleNumber));

                    break;
                case ReputationType.Castle:

                    boxType = BoxType.Castle;
                    foodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationCastleFoodId));
                    title = (TitleType) await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationCastleTitleNumber));

                    break;
                case ReputationType.Village:

                    boxType = BoxType.Village;
                    foodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationVillageFoodId));
                    title = (TitleType) await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.ReputationVillageTitleNumber));

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var food = await _mediator.Send(new GetFoodByIncIdQuery(foodIncId));

            var embed = new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя {emotes.GetEmote(type.Emote(userReputation.Amount))} {userReputation.Amount} " +
                    $"репутации в **{type.Location().Localize(true)}**." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Награды за получение репутации",
                    $"{emotes.GetEmote(userReputation.Amount >= 500 ? "Checkmark" : "List")} За {emotes.GetEmote(type.Emote(500))} `500` репутации ты получишь {emotes.GetEmote(boxType.EmoteName())} {boxAmount} {_local.Localize(LocalizationCategoryType.Box, boxType.ToString(), boxAmount)}." +
                    $"\n{emotes.GetEmote(userReputation.Amount >= 1000 ? "Checkmark" : "List")} За {emotes.GetEmote(type.Emote(1000))} `1000` репутации ты получишь {emotes.GetEmote(CurrencyType.Pearl.ToString())} {rep1000PearlAmount} {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), rep1000PearlAmount)}." +
                    $"\n{emotes.GetEmote(userReputation.Amount >= 2000 ? "Checkmark" : "List")} За {emotes.GetEmote(type.Emote(2000))} `2000` репутации ты получишь {emotes.GetEmote("Recipe")} {_local.Localize(LocalizationCategoryType.Food, food.Name)}." +
                    $"\n{emotes.GetEmote(userReputation.Amount >= 5000 ? "Checkmark" : "List")} За {emotes.GetEmote(type.Emote(5000))} `5000` репутации ты получишь {emotes.GetEmote(CurrencyType.Pearl.ToString())} {rep5000PearlAmount} {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), rep5000PearlAmount)}." +
                    $"\n{emotes.GetEmote(userReputation.Amount >= 10000 ? "Checkmark" : "List")} За {emotes.GetEmote(type.Emote(10000))} `10000` репутации ты получишь титул {emotes.GetEmote(title.EmoteName())} {title.Localize()}.");

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

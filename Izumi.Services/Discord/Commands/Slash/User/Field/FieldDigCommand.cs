using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Field.Commands;
using Izumi.Services.Game.Field.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Field
{
    public record FieldDigCommand(SocketSlashCommand Command) : IRequest;

    public class FieldDigHandler : IRequestHandler<FieldDigCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldDigHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FieldDigCommand request, CancellationToken cancellationToken)
        {
            var number = (uint) (long) request.Command.Data
                .Options.First()
                .Options.Single(x => x.Name == "номер").Value;

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userField = await _mediator.Send(new GetUserFieldQuery(user.Id, number));

            var embed = new EmbedBuilder()
                .WithAuthor("Выкапывание урожая")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Field)));

            if (userField.State == FieldStateType.Empty)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "на этой клетке земли ничего не растет.");
            }
            else
            {
                var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.EnergyCostFieldDig));

                await _mediator.Send(new ResetUserFieldCommand(user.Id, number));
                await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно выкопал {emotes.GetEmote(userField.Seed.Name)} " +
                        $"{_local.Localize(LocalizationCategoryType.Seed, userField.Seed.Name)} с этой клетки земли, " +
                        "теперь она свободна." +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField("Расход энергии",
                        $"{emotes.GetEmote("Energy")} {energyCost} " +
                        $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost)}",
                        true);
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

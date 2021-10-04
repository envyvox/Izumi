using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Casino
{
    public record BetCommand(SocketSlashCommand Command) : IRequest;

    public class BetHandler : IRequestHandler<BetCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public BetHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(BetCommand request, CancellationToken cancellationToken)
        {
            var betAmount = (uint) (long) request.Command.Data.Options.First().Value;
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "данный функционал находится в разработке. " +
                    "Следи за каналом <#750624435702333460> чтобы быть в курсе всех обновлений.")));
        }
    }
}

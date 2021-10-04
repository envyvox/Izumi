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

namespace Izumi.Services.Discord.Commands.Slash.Market
{
    public record MarketSellCommand(SocketSlashCommand Command) : IRequest;

    public class MarketSellHandler : IRequestHandler<MarketSellCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MarketSellHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(MarketSellCommand request, CancellationToken ct)
        {
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

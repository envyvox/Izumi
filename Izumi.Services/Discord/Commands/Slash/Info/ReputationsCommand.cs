using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
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

            return await _mediator.Send(new RespondEmbedCommand(request.Command, new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "данный функционал находится в разработке. " +
                    "Следи за каналом <#750624435702333460> чтобы быть в курсе всех обновлений.")));
        }
    }
}

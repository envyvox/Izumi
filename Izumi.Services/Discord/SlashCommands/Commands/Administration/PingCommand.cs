using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Services.Discord.Embed;
using MediatR;
using IRequest = MediatR.IRequest;

namespace Izumi.Services.Discord.SlashCommands.Commands.Administration
{
    public record PingCommand(SocketSlashCommand Command) : IRequest;

    public class PingHandler : IRequestHandler<PingCommand>
    {
        private readonly IMediator _mediator;

        public PingHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            var embed = new EmbedBuilder()
                .WithDescription("Pong!");

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed, true));
        }
    }
}

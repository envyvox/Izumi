using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Embed
{
    public record RespondEmbedCommand(
            SocketSlashCommand Command,
            EmbedBuilder EmbedBuilder,
            MessageComponent Component = null,
            string Text = null)
        : IRequest;

    public class RespondEmbedHandler : IRequestHandler<RespondEmbedCommand>
    {
        private readonly IMediator _mediator;

        public RespondEmbedHandler(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RespondEmbedCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            var embed = request.EmbedBuilder
                .WithUserColor(user.CommandColor);

            await request.Command.FollowupAsync(request.Text,
                embed: embed.Build(),
                component: request.Component);

            return Unit.Value;
        }
    }
}
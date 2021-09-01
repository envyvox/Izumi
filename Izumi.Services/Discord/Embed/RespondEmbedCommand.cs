using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Embed
{
    public record RespondEmbedCommand(
            SocketSlashCommand Command,
            EmbedBuilder Builder,
            bool Ephemeral = false,
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

            var embed = request.Builder
                .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)))
                .Build();

            await request.Command.FollowupAsync(
                text: request.Text,
                embeds: new[] { embed },
                isTTS: false,
                ephemeral: request.Ephemeral,
                allowedMentions: AllowedMentions.None,
                options: null,
                component: request.Component);

            return Unit.Value;
        }
    }
}

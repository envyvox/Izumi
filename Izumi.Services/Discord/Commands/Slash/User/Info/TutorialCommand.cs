using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Game.Tutorial.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info
{
    public record TutorialCommand(SocketSlashCommand Command) : IRequest;

    public class TutorialHandler : IRequestHandler<TutorialCommand>
    {
        private readonly IMediator _mediator;

        public TutorialHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(TutorialCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userTutorialStep = await _mediator.Send(new GetUserTutorialStepQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor(userTutorialStep.Name())
                .WithDescription(userTutorialStep.Description());

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

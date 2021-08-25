using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Services.Discord.SlashCommands.Commands.Administration;
using Izumi.Services.Discord.SlashCommands.Commands.User.Info;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record InteractionCreated(SocketInteraction Interaction) : IRequest;

    public class InteractionCreatedHandler : IRequestHandler<InteractionCreated>
    {
        private readonly IMediator _mediator;

        public InteractionCreatedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(InteractionCreated request, CancellationToken ct)
        {
            return request.Interaction switch
            {
                SocketSlashCommand command => command.Data.Name switch
                {
                    "ping" => await _mediator.Send(new PingCommand(command)),
                    "профиль" => await _mediator.Send(new ProfileCommand(command)),
                    _ => Unit.Value
                },
                SocketMessageComponent component => component.Data.CustomId switch
                {
                    _ => Unit.Value
                },
                _ => Unit.Value
            };
        }
    }
}

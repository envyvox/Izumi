using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Settings
{
    public record SettingsCommand(SocketSlashCommand Command) : IRequest;

    public class SettingsCommandHandler : IRequestHandler<SettingsCommand>
    {
        private readonly IMediator _mediator;

        public SettingsCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingsCommand request, CancellationToken ct)
        {
            return request.Command.Data.Options.First().Name switch
            {
                "роли-титула" => await _mediator.Send(new SettingsAutoTitleRoleCommand(request.Command)),
                "цвет-команд" => await _mediator.Send(new SettingsCommandColorCommand(request.Command)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

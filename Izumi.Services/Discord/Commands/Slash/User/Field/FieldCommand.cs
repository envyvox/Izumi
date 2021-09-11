using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Field
{
    public record FieldCommand(SocketSlashCommand Command) : IRequest;

    public class FieldHandler : IRequestHandler<FieldCommand>
    {
        private readonly IMediator _mediator;

        public FieldHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(FieldCommand request, CancellationToken ct)
        {
            return request.Command.Data.Options.First().Name switch
            {
                "информация" => await _mediator.Send(new FieldInfoCommand(request.Command)),
                "купить" => await _mediator.Send(new FieldBuyCommand(request.Command)),
                "посадить" => await _mediator.Send(new FieldPlantCommand(request.Command)),
                "полить" => await _mediator.Send(new FieldWaterCommand(request.Command)),
                "собрать" => await _mediator.Send(new FieldCollectCommand(request.Command)),
                "выкопать" => await _mediator.Send(new FieldDigCommand(request.Command)),
                _ => Unit.Value
            };
        }
    }
}

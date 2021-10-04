using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Casino
{
    public record LotteryCommand(SocketSlashCommand Command) : IRequest;

    public class LotteryHandler : IRequestHandler<LotteryCommand>
    {
        private readonly IMediator _mediator;

        public LotteryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(LotteryCommand request, CancellationToken cancellationToken)
        {
            return request.Command.Data.Options.First().Name switch
            {
                "информация" => await _mediator.Send(new LotteryInfoCommand(request.Command)),
                "купить" => await _mediator.Send(new LotteryBuyCommand(request.Command)),
                "подарить" => await _mediator.Send(new LotteryGiftCommand(request.Command)),
                _ => Unit.Value
            };
        }
    }
}

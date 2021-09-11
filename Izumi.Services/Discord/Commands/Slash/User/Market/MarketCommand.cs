using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Market
{
    public record MarketCommand(SocketSlashCommand Command) : IRequest;

    public class MarketHandler : IRequestHandler<MarketCommand>
    {
        private readonly IMediator _mediator;

        public MarketHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MarketCommand request, CancellationToken ct)
        {
            return request.Command.Data.Options.First().Name switch
            {
                "информация" => await _mediator.Send(new MarketInfoCommand(request.Command)),
                "купить" => await _mediator.Send(new MarketBuyCommand(request.Command)),
                "продать" => await _mediator.Send(new MarketSellCommand(request.Command)),
                _ => Unit.Value
            };
        }
    }
}

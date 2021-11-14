using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Services.Discord.Commands.Slash.Shop.Buy;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Shop
{
    public record ShopBuyCommand(SocketSlashCommand Command) : IRequest;

    public class ShopBuyHandler : IRequestHandler<ShopBuyCommand>
    {
        private readonly IMediator _mediator;

        public ShopBuyHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ShopBuyCommand request, CancellationToken cancellationToken)
        {
            return (string) request.Command.Data.Options.First() switch
            {
                "seed" => await _mediator.Send(new ShopSeedBuyCommand(request.Command)),
                "product" => await _mediator.Send(new ShopProductBuyCommand(request.Command)),
                _ => Unit.Value
            };
        }
    }
}

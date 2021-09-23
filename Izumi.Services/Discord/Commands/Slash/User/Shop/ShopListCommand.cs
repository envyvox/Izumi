using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Services.Discord.Commands.Slash.User.Shop.List;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Shop
{
    public record ShopListCommand(SocketSlashCommand Command) : IRequest;

    public class ShopListHandler : IRequestHandler<ShopListCommand>
    {
        private readonly IMediator _mediator;

        public ShopListHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ShopListCommand request, CancellationToken cancellationToken)
        {
            return (string) request.Command.Data.Options.First() switch
            {
                "seed" => await _mediator.Send(new ShopSeedListCommand(request.Command)),
                "product" => await _mediator.Send(new ShopProductListCommand(request.Command)),
                "recipe" => await _mediator.Send(new ShopRecipeListCommand(request.Command)),
                "banner" => await _mediator.Send(new ShopBannerListCommand(request.Command)),
                _ => Unit.Value
            };
        }
    }
}

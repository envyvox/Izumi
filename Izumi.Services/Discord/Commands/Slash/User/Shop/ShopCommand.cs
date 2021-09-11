using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Services.Discord.Commands.Slash.User.Shop.Buy;
using Izumi.Services.Discord.Commands.Slash.User.Shop.List;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Shop
{
    public record ShopCommand(SocketSlashCommand Command) : IRequest;

    public class ShopHandler : IRequestHandler<ShopCommand>
    {
        private readonly IMediator _mediator;

        public ShopHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ShopCommand request, CancellationToken ct)
        {
            switch ((string) request.Command.Data.Options.First().Options.First())
            {
                case "seed":

                    if (request.Command.Data.Options.First().Name == "посмотреть")
                        return await _mediator.Send(new ShopSeedListCommand(request.Command));
                    return await _mediator.Send(new ShopSeedBuyCommand(request.Command));

                case "product":

                    if (request.Command.Data.Options.First().Name == "посмотреть")
                        return await _mediator.Send(new ShopProductListCommand(request.Command));
                    return await _mediator.Send(new ShopProductBuyCommand(request.Command));

                case "recipe":

                    if (request.Command.Data.Options.First().Name == "посмотреть")
                        return await _mediator.Send(new ShopRecipeListCommand(request.Command));
                    return await _mediator.Send(new ShopRecipeBuyCommand(request.Command));

                case "banner":

                    if (request.Command.Data.Options.First().Name == "посмотреть")
                        return await _mediator.Send(new ShopBannerListCommand(request.Command));
                    return await _mediator.Send(new ShopBannerBuyCommand(request.Command));
            }

            return Unit.Value;
        }
    }
}

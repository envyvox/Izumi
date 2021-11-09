using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Calculation
{
    public record GetCraftingPriceQuery(
            uint CostPrice,
            uint Amount = 1)
        : IRequest<uint>;

    public class GetCraftingPriceHandler : IRequestHandler<GetCraftingPriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetCraftingPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetCraftingPriceQuery request, CancellationToken ct)
        {
            var craftingPercent = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.CraftingPricePercent));

            // cast from double to long is required
            var craftingPrice = (long) (request.CostPrice / 100.0 * craftingPercent < 1
                ? 1
                : request.CostPrice / 100.0 * craftingPercent);

            return (uint) (request.Amount > 0
                ? craftingPrice * request.Amount
                : craftingPrice);
        }
    }
}
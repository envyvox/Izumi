using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Calculation
{
    public record GetMarketPriceQuery(
            MarketCategoryType Category,
            uint CostPrice,
            uint CraftingPrice)
        : IRequest<uint>;

    public class GetMarketPriceHandler : IRequestHandler<GetMarketPriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetMarketPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetMarketPriceQuery request, CancellationToken ct)
        {
            var markup = await _mediator.Send(new GetWorldPropertyValueQuery(request.Category switch
            {
                MarketCategoryType.Crafting => WorldPropertyType.MarketMarkupCrafting,
                MarketCategoryType.Alcohol => WorldPropertyType.MarketMarkupAlcohol,
                MarketCategoryType.Drink => WorldPropertyType.MarketMarkupDrink,
                MarketCategoryType.Food => WorldPropertyType.MarketMarkupFood,
                _ => throw new ArgumentOutOfRangeException()
            }));

            return (uint) (request.CostPrice + request.CraftingPrice +
                           (request.CostPrice + request.CraftingPrice) / 100.0 * markup);
        }
    }
}
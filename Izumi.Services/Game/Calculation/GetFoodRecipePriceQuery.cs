using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Calculation
{
    public record GetFoodRecipePriceQuery(uint CostPrice, uint CraftingPrice) : IRequest<uint>;

    public class GetFoodRecipePriceHandler : IRequestHandler<GetFoodRecipePriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetFoodRecipePriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetFoodRecipePriceQuery request, CancellationToken ct)
        {
            var marketPrice = await _mediator.Send(new GetMarketPriceQuery(
                MarketCategoryType.Food, request.CostPrice, request.CraftingPrice));
            var profit = await _mediator.Send(new GetProfitQuery(
                request.CostPrice, request.CraftingPrice, marketPrice));
            var paybackSales = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.RecipePaybackSales));

            return profit * paybackSales;
        }
    }
}
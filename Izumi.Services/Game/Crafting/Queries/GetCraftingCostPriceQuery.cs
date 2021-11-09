using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Crafting.Models;
using Izumi.Services.Game.Ingredient.Queries;
using MediatR;

namespace Izumi.Services.Game.Crafting.Queries
{
    public record GetCraftingCostPriceQuery(List<CraftingIngredientDto> Ingredients) : IRequest<uint>;

    public class GetCraftingCostPriceHandler : IRequestHandler<GetCraftingCostPriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetCraftingCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetCraftingCostPriceQuery request, CancellationToken ct)
        {
            uint costPrice = 0;

            foreach (var ingredient in request.Ingredients)
            {
                var ingredientCostPrice = await _mediator.Send(new GetIngredientCostPriceQuery(
                    ingredient.Category, ingredient.IngredientId));

                costPrice += ingredientCostPrice * ingredient.Amount;
            }

            return costPrice;
        }
    }
}
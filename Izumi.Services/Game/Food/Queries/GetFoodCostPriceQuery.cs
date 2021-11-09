using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Food.Models;
using Izumi.Services.Game.Ingredient.Queries;
using MediatR;

namespace Izumi.Services.Game.Food.Queries
{
    public record GetFoodCostPriceQuery(List<FoodIngredientDto> Ingredients) : IRequest<uint>;

    public class GetFoodCostPriceHandler : IRequestHandler<GetFoodCostPriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetFoodCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetFoodCostPriceQuery request, CancellationToken ct)
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
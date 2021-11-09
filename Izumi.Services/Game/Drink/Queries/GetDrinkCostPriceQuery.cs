using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Drink.Models;
using Izumi.Services.Game.Ingredient.Queries;
using MediatR;

namespace Izumi.Services.Game.Drink.Queries
{
    public record GetDrinkCostPriceQuery(List<DrinkIngredientDto> Ingredients) : IRequest<uint>;

    public class GetDrinkCostPriceHandler : IRequestHandler<GetDrinkCostPriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetDrinkCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetDrinkCostPriceQuery request, CancellationToken ct)
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
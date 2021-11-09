using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Alcohol.Models;
using Izumi.Services.Game.Ingredient.Queries;
using MediatR;

namespace Izumi.Services.Game.Alcohol.Queries
{
    public record GetAlcoholCostPriceQuery(List<AlcoholIngredientDto> Ingredients) : IRequest<uint>;

    public class GetAlcoholCostPriceHandler : IRequestHandler<GetAlcoholCostPriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetAlcoholCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetAlcoholCostPriceQuery request, CancellationToken ct)
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
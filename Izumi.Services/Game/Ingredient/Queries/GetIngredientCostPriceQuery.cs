using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.Seafood.Queries;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Queries
{
    public record GetIngredientCostPriceQuery(
            IngredientCategoryType Category,
            Guid IngredientId)
        : IRequest<uint>;

    public class GetIngredientCostPriceHandler : IRequestHandler<GetIngredientCostPriceQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetIngredientCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetIngredientCostPriceQuery request, CancellationToken ct)
        {
            switch (request.Category)
            {
                case IngredientCategoryType.Gathering:

                    var gathering = await _mediator.Send(new GetGatheringByIdQuery(request.IngredientId));
                    return gathering.Price;

                case IngredientCategoryType.Product:

                    var product = await _mediator.Send(new GetProductByIdQuery(request.IngredientId));
                    return product.Price;

                case IngredientCategoryType.Crafting:

                    var crafting = await _mediator.Send(new GetCraftingByIdQuery(request.IngredientId));
                    return await _mediator.Send(new GetCraftingCostPriceQuery(crafting.Ingredients));

                case IngredientCategoryType.Alcohol:

                    var alcohol = await _mediator.Send(new GetAlcoholByIdQuery(request.IngredientId));
                    return await _mediator.Send(new GetAlcoholCostPriceQuery(alcohol.Ingredients));

                case IngredientCategoryType.Drink:

                    var drink = await _mediator.Send(new GetDrinkByIdQuery(request.IngredientId));
                    return await _mediator.Send(new GetDrinkCostPriceQuery(drink.Ingredients));

                case IngredientCategoryType.Crop:

                    var crop = await _mediator.Send(new GetCropByIdQuery(request.IngredientId));
                    return crop.Price;

                case IngredientCategoryType.Food:

                    var food = await _mediator.Send(new GetFoodByIdQuery(request.IngredientId));
                    return await _mediator.Send(new GetFoodCostPriceQuery(food.Ingredients));

                case IngredientCategoryType.Seafood:

                    var seafood = await _mediator.Send(new GetSeafoodByIdQuery(request.IngredientId));
                    return seafood.Price;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
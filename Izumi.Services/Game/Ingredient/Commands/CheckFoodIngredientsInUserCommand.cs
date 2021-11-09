using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Food.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record CheckFoodIngredientsInUserCommand(
            long UserId,
            List<FoodIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class CheckFoodIngredientsInUserHandler : IRequestHandler<CheckFoodIngredientsInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckFoodIngredientsInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckFoodIngredientsInUserCommand request, CancellationToken ct)
        {
            foreach (var ingredient in request.Ingredients)
            {
                await _mediator.Send(new CheckIngredientAmountInUserCommand(
                    request.UserId, ingredient.Category, ingredient.IngredientId, ingredient.Amount,
                    request.CraftingAmount));
            }

            return Unit.Value;
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Food.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record RemoveFoodIngredientsFromUserCommand(
            long UserId,
            List<FoodIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class RemoveFoodIngredientsFromUserHandler : IRequestHandler<RemoveFoodIngredientsFromUserCommand>
    {
        private readonly IMediator _mediator;

        public RemoveFoodIngredientsFromUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveFoodIngredientsFromUserCommand request, CancellationToken ct)
        {
            foreach (var ingredient in request.Ingredients)
            {
                await _mediator.Send(new RemoveIngredientFromUserCommand(
                    request.UserId, ingredient.Category, ingredient.IngredientId, ingredient.Amount,
                    request.CraftingAmount));
            }

            return Unit.Value;
        }
    }
}
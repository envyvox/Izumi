using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Drink.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record RemoveDrinkIngredientsFromUserCommand(
            long UserId,
            List<DrinkIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class RemoveDrinkIngredientsFromUserHandler : IRequestHandler<RemoveDrinkIngredientsFromUserCommand>
    {
        private readonly IMediator _mediator;

        public RemoveDrinkIngredientsFromUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveDrinkIngredientsFromUserCommand request, CancellationToken ct)
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
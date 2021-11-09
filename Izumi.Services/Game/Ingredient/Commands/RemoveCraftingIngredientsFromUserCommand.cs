using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Crafting.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record RemoveCraftingIngredientsFromUserCommand(
            long UserId,
            List<CraftingIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class RemoveCraftingIngredientsFromUserHandler : IRequestHandler<RemoveCraftingIngredientsFromUserCommand>
    {
        private readonly IMediator _mediator;

        public RemoveCraftingIngredientsFromUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveCraftingIngredientsFromUserCommand request, CancellationToken ct)
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Crafting.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record CheckCraftingIngredientsInUserCommand(
            long UserId,
            List<CraftingIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class CheckCraftingIngredientsInUserHandler : IRequestHandler<CheckCraftingIngredientsInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckCraftingIngredientsInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckCraftingIngredientsInUserCommand request, CancellationToken ct)
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
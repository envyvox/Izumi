using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record RemoveAlcoholIngredientsFromUserCommand(
            long UserId,
            List<AlcoholIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class RemoveAlcoholIngredientsFromUserHandler : IRequestHandler<RemoveAlcoholIngredientsFromUserCommand>
    {
        private readonly IMediator _mediator;

        public RemoveAlcoholIngredientsFromUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveAlcoholIngredientsFromUserCommand request, CancellationToken ct)
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
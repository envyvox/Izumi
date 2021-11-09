using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record CheckAlcoholIngredientsInUserCommand(
            long UserId,
            List<AlcoholIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class CheckAlcoholIngredientsInUserHandler : IRequestHandler<CheckAlcoholIngredientsInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckAlcoholIngredientsInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAlcoholIngredientsInUserCommand request, CancellationToken ct)
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
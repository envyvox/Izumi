﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Game.Drink.Models;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Commands
{
    public record CheckDrinkIngredientsInUserCommand(
            long UserId,
            List<DrinkIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest;

    public class CheckDrinkIngredientsInUserHandler : IRequestHandler<CheckDrinkIngredientsInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckDrinkIngredientsInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckDrinkIngredientsInUserCommand request, CancellationToken ct)
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
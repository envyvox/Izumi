﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Food.Models;
using Izumi.Services.Game.Localization;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Queries
{
    public record GetFoodIngredientsStringQuery(
            List<FoodIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest<string>;

    public class GetFoodIngredientsStringHandler : IRequestHandler<GetFoodIngredientsStringQuery, string>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetFoodIngredientsStringHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<string> Handle(GetFoodIngredientsStringQuery request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var ingredientsString = string.Empty;

            foreach (var ingredient in request.Ingredients)
            {
                var ing = await _mediator.Send(new GetIngredientByIdQuery(
                    ingredient.Category, ingredient.IngredientId));

                ingredientsString +=
                    $"{emotes.GetEmote(ing.Name)} {ingredient.Amount * request.CraftingAmount} " +
                    $"{_local.Localize(ing.Category, ing.Name, ingredient.Amount * request.CraftingAmount)}, ";
            }

            return ingredientsString.RemoveFromEnd(2);
        }
    }
}
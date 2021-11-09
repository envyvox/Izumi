using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Alcohol.Models;
using Izumi.Services.Game.Localization;
using MediatR;

namespace Izumi.Services.Game.Ingredient.Queries
{
    public record GetAlcoholIngredientsStringQuery(
            List<AlcoholIngredientDto> Ingredients,
            uint CraftingAmount = 1)
        : IRequest<string>;

    public class GetAlcoholIngredientsStringHandler : IRequestHandler<GetAlcoholIngredientsStringQuery, string>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetAlcoholIngredientsStringHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<string> Handle(GetAlcoholIngredientsStringQuery request, CancellationToken ct)
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
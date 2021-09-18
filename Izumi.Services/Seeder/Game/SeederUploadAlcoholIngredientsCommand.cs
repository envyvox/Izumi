using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Alcohol.Commands;
using Izumi.Services.Game.Ingredient.Models;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadAlcoholIngredientsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadAlcoholIngredientsHandler
        : IRequestHandler<SeederUploadAlcoholIngredientsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadAlcoholIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadAlcoholIngredientsCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateAlcoholIngredientsCommand[]
            {
                new("Beer", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Wheat", 1),
                    new(IngredientCategoryType.Crop, "Hops", 1)
                }),
                new("BlueberryBeer", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Alcohol, "Beer", 1),
                    new(IngredientCategoryType.Crop, "Blueberry", 1)
                }),
                new("Sake", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Rice", 1)
                }),
                new("Wine", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Grape", 2)
                })
            };

            foreach (var createAlcoholIngredientsCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createAlcoholIngredientsCommand);

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}

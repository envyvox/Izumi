using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Crafting.Commands;
using Izumi.Services.Game.Ingredient.Models;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadCraftingIngredientsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadCraftingIngredientsHandler
        : IRequestHandler<SeederUploadCraftingIngredientsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadCraftingIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadCraftingIngredientsCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateCraftingIngredientsCommand[]
            {
                new("Stick", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Gathering, "Wood", 1)
                }),
                new("Board", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Gathering, "Wood", 1)
                }),
                new("IronBar", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Gathering, "Iron", 2)
                }),
                new("GoldBar", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Gathering, "Gold", 2)
                }),
                new("Cloth", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Gathering, "Flax", 10)
                }),
                new("Rope", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Gathering, "Grass", 10)
                }),
                new("WheatFlour", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Wheat", 5)
                }),
                new("Oil", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Sunflower", 1)
                }),
                new("Vinegar", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Alcohol, "Wine", 1)
                }),
                new("Sugar", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Beet", 3)
                }),
                new("Mayonnaise", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Egg", 2)
                }),
                new("Cheese", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Milk", 1)
                })
            };

            foreach (var createCraftingIngredientsCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createCraftingIngredientsCommand);

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

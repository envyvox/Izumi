using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Food.Commands;
using Izumi.Services.Game.Ingredient.Models;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadFoodIngredientsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadFoodIngredientsHandler
        : IRequestHandler<SeederUploadFoodIngredientsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadFoodIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadFoodIngredientsCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateFoodIngredientsCommand[]
            {
                new("Tortilla", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Corn", 1)
                }),
                new("Bread", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1)
                }),
                new("FriedEgg", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Egg", 1)
                }),
                new("BeanHotpot", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "GreenBean", 2)
                }),
                new("Spaghetti", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1)
                }),
                new("VegetableMedley", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Tomato", 1),
                    new(IngredientCategoryType.Crop, "Beet", 1)
                }),
                new("EggplantParmesan", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Eggplant", 1),
                    new(IngredientCategoryType.Crop, "Tomato", 1)
                }),
                new("Pancakes", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Egg", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1)
                }),
                new("Bruschetta", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Tomato", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1),
                    new(IngredientCategoryType.Food, "Bread", 1)
                }),
                new("Hashbrowns", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Potato", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("RedPlate", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "RedCabbage", 1),
                    new(IngredientCategoryType.Crop, "Radish", 1)
                }),
                new("Omelet", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Egg", 1),
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Crop, "Eggplant", 1)
                }),
                new("PepperPoppers", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "HotPepper", 1),
                    new(IngredientCategoryType.Crafting, "Cheese", 1)
                }),
                new("GarlicOil", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Garlic", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("RadishSalad", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Radish", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1),
                    new(IngredientCategoryType.Crafting, "Vinegar", 1)
                }),
                new("FarmBreakfast", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Parsnip", 1),
                    new(IngredientCategoryType.Food, "Omelet", 1)
                }),
                new("SuperMeal", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "BokChoy", 1),
                    new(IngredientCategoryType.Crop, "Cranberry", 1),
                    new(IngredientCategoryType.Crop, "Artichoke", 1)
                }),
                new("Pizza", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Tomato", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Cheese", 1)
                }),
                new("ArtichokeDip", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Artichoke", 1),
                    new(IngredientCategoryType.Product, "Milk", 1)
                }),
                new("ParsnipSoup", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Parsnip", 1),
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Crafting, "Vinegar", 1)
                }),
                new("CranberrySauce", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Cranberry", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("CheeseCauliflower", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Cauliflower", 1),
                    new(IngredientCategoryType.Crafting, "Cheese", 1)
                }),
                new("Coleslaw", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "RedCabbage", 1),
                    new(IngredientCategoryType.Crafting, "Vinegar", 1),
                    new(IngredientCategoryType.Crafting, "Mayonnaise", 1)
                }),
                new("Cookie", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Egg", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("ChocolateCake", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Egg", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("IceCream", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("GlazedYams", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Yam", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("BlueberryTart", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Blueberry", 1),
                    new(IngredientCategoryType.Product, "Egg", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("PumpkinSoup", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Pumpkin", 1),
                    new(IngredientCategoryType.Product, "Milk", 1)
                }),
                new("AutumnTale", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Yam", 1),
                    new(IngredientCategoryType.Crop, "Pumpkin", 1)
                }),
                new("CompleteBreakfast", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Food, "FriedEgg", 1),
                    new(IngredientCategoryType.Food, "Hashbrowns", 1),
                    new(IngredientCategoryType.Food, "Pancakes", 1)
                }),
                new("RhubarbPie", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Rhubarb", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("PinkCake", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Melon", 1),
                    new(IngredientCategoryType.Product, "Egg", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("PumpkinPie", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Pumpkin", 1),
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("Onigiri", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Rice", 1)
                }),
                new("CurryRice", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Rice", 1),
                    new(IngredientCategoryType.Crop, "HotPepper", 1),
                    new(IngredientCategoryType.Crop, "Tomato", 1)
                }),
                new("Oyakodon", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Food, "Omelet", 1),
                    new(IngredientCategoryType.Crop, "Rice", 1)
                }),
                new("ParsnipChips", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Parsnip", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("GarlicRice", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Rice", 1),
                    new(IngredientCategoryType.Crop, "Garlic", 1)
                }),
                new("HerbsEggs", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Food, "FriedEgg", 1),
                    new(IngredientCategoryType.Crop, "GreenBean", 1),
                    new(IngredientCategoryType.Crop, "Cauliflower", 1)
                }),
                new("SpringSalad", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Parsnip", 1),
                    new(IngredientCategoryType.Crop, "Rhubarb", 1),
                    new(IngredientCategoryType.Crop, "GreenBean", 1)
                }),
                new("CreamSoup", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Garlic", 1),
                    new(IngredientCategoryType.Crop, "Cauliflower", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1),
                    new(IngredientCategoryType.Food, "Bread", 1)
                }),
                new("BakedGarlic", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Garlic", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1),
                    new(IngredientCategoryType.Food, "Bread", 1)
                }),
                new("RiceBalls", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Rice", 1),
                    new(IngredientCategoryType.Crafting, "Cheese", 1),
                    new(IngredientCategoryType.Crop, "Potato", 1)
                }),
                new("CreamyPuree", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Potato", 1),
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Crop, "Parsnip", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("MochiCakes", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crafting, "Vinegar", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crop, "Strawberry", 1)
                }),
                new("HokkaidoBuns", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Strawberry", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Crop, "Rhubarb", 1)
                }),
                new("FruitIce", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crafting, "Sugar", 1),
                    new(IngredientCategoryType.Crop, "Strawberry", 1)
                }),
                new("ChakinSibori", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "GreenBean", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1),
                    new(IngredientCategoryType.Product, "Egg", 1)
                }),
                new("GlazedParsnip", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Parsnip", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1),
                    new(IngredientCategoryType.Crafting, "Vinegar", 1)
                }),
                new("StrawberryCake", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crop, "Strawberry", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("StrawberryRhubarbJam", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Strawberry", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1),
                    new(IngredientCategoryType.Crop, "Rhubarb", 1)
                }),
                new("Tofu", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "GreenBean", 1),
                    new(IngredientCategoryType.Crafting, "Cheese", 1)
                }),
                new("GlazedMelon", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Melon", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("MelonBlueberrySorbet", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Melon", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1),
                    new(IngredientCategoryType.Crop, "Blueberry", 1)
                }),
                new("PickledMelon", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Melon", 1),
                    new(IngredientCategoryType.Crop, "HotPepper", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("PumpkinMelonCake", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Melon", 1),
                    new(IngredientCategoryType.Crop, "Pumpkin", 1),
                    new(IngredientCategoryType.Product, "Egg", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1)
                }),
                new("SpicyTofu", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Food, "Tofu", 1),
                    new(IngredientCategoryType.Crop, "HotPepper", 1),
                    new(IngredientCategoryType.Crop, "Corn", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("RedSalad", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "RedCabbage", 1),
                    new(IngredientCategoryType.Crop, "Corn", 1),
                    new(IngredientCategoryType.Crop, "Tomato", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("RedSaladOmelet", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Food, "RedSalad", 1),
                    new(IngredientCategoryType.Food, "Omelet", 1)
                }),
                new("CornSoup", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Corn", 1),
                    new(IngredientCategoryType.Product, "Milk", 1),
                    new(IngredientCategoryType.Crafting, "WheatFlour", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("Burrito", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Food, "Tortilla", 1),
                    new(IngredientCategoryType.Crop, "HotPepper", 1),
                    new(IngredientCategoryType.Crop, "Corn", 1),
                    new(IngredientCategoryType.Crop, "Tomato", 1)
                }),
                new("CornSalad", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Corn", 1),
                    new(IngredientCategoryType.Gathering, "Mushroom", 1),
                    new(IngredientCategoryType.Crafting, "Mayonnaise", 1),
                    new(IngredientCategoryType.Product, "Egg", 1)
                }),
                new("Harunosarada", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Corn", 1),
                    new(IngredientCategoryType.Crop, "RedCabbage", 1),
                    new(IngredientCategoryType.Crop, "Radish", 1),
                    new(IngredientCategoryType.Crafting, "Vinegar", 1)
                }),
                new("SpaghettiSalad", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Tomato", 1),
                    new(IngredientCategoryType.Food, "Spaghetti", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1)
                }),
                new("AgeTofu", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Tomato", 1),
                    new(IngredientCategoryType.Food, "Tofu", 1),
                    new(IngredientCategoryType.Crafting, "Oil", 1),
                    new(IngredientCategoryType.Crafting, "Sugar", 1)
                }),
                new("SpicySpaghetti", new List<CreateIngredientDto>
                {
                    new(IngredientCategoryType.Crop, "Tomato", 1),
                    new(IngredientCategoryType.Food, "Tofu", 1),
                    new(IngredientCategoryType.Food, "Spaghetti", 1),
                    new(IngredientCategoryType.Crop, "HotPepper", 1)
                })
            };

            foreach (var createFoodIngredientsCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createFoodIngredientsCommand);

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

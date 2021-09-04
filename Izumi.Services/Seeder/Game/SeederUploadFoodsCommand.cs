using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Food.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadFoodsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadFoodsHandler : IRequestHandler<SeederUploadFoodsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadFoodsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadFoodsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateFoodCommand[]
            {
                new("Tortilla", FoodCategoryType.Newbie, true, false),
                new("Bread", FoodCategoryType.Newbie, true, false),
                new("FriedEgg", FoodCategoryType.Newbie, false, false),
                new("BeanHotpot", FoodCategoryType.Newbie, true, false),
                new("Spaghetti", FoodCategoryType.Newbie, true, false),
                new("VegetableMedley", FoodCategoryType.Newbie, true, false),
                new("EggplantParmesan", FoodCategoryType.Newbie, true, false),
                new("Pancakes", FoodCategoryType.Newbie, true, false),
                new("Bruschetta", FoodCategoryType.Newbie, true, false),
                new("Hashbrowns", FoodCategoryType.Newbie, true, false),
                new("RedPlate", FoodCategoryType.Newbie, true, false),
                new("Omelet", FoodCategoryType.Newbie, true, false),
                new("PepperPoppers", FoodCategoryType.Newbie, true, false),
                new("GarlicOil", FoodCategoryType.Newbie, true, false),
                new("RadishSalad", FoodCategoryType.Newbie, true, false),
                new("FarmBreakfast", FoodCategoryType.Newbie, true, false),
                new("SuperMeal", FoodCategoryType.Newbie, true, false),
                new("Pizza", FoodCategoryType.Newbie, true, false),
                new("ArtichokeDip", FoodCategoryType.Newbie, true, false),
                new("ParsnipSoup", FoodCategoryType.Newbie, true, false),
                new("CranberrySauce", FoodCategoryType.Newbie, true, false),
                new("CheeseCauliflower", FoodCategoryType.Newbie, true, false),
                new("Coleslaw", FoodCategoryType.Newbie, true, false),
                new("Cookie", FoodCategoryType.Newbie, true, false),
                new("ChocolateCake", FoodCategoryType.Newbie, false, false),
                new("IceCream", FoodCategoryType.Newbie, true, false),
                new("GlazedYams", FoodCategoryType.Newbie, true, false),
                new("BlueberryTart", FoodCategoryType.Newbie, true, false),
                new("PumpkinSoup", FoodCategoryType.Newbie, true, false),
                new("AutumnTale", FoodCategoryType.Newbie, true, false),
                new("CompleteBreakfast", FoodCategoryType.Newbie, true, false),
                new("RhubarbPie", FoodCategoryType.Newbie, true, false),
                new("PinkCake", FoodCategoryType.Newbie, true, false),
                new("PumpkinPie", FoodCategoryType.Newbie, true, false),
                new("Onigiri", FoodCategoryType.Newbie, false, false),
                new("CurryRice", FoodCategoryType.Newbie, true, false),
                new("Oyakodon", FoodCategoryType.Newbie, false, false),
                new("ParsnipChips", FoodCategoryType.Newbie, true, false),
                new("GarlicRice", FoodCategoryType.Newbie, true, false),
                new("HerbsEggs", FoodCategoryType.Newbie, true, false),
                new("SpringSalad", FoodCategoryType.Newbie, true, false),
                new("CreamSoup", FoodCategoryType.Newbie, true, false),
                new("BakedGarlic", FoodCategoryType.Newbie, true, false),
                new("RiceBalls", FoodCategoryType.Newbie, true, false),
                new("CreamyPuree", FoodCategoryType.Newbie, true, false),
                new("MochiCakes", FoodCategoryType.Newbie, true, false),
                new("HokkaidoBuns", FoodCategoryType.Newbie, false, false),
                new("FruitIce", FoodCategoryType.Newbie, true, false),
                new("ChakinSibori", FoodCategoryType.Newbie, false, false),
                new("GlazedParsnip", FoodCategoryType.Newbie, true, false),
                new("StrawberryCake", FoodCategoryType.Newbie, false, false),
                new("StrawberryRhubarbJam", FoodCategoryType.Newbie, false, false),
                new("Tofu", FoodCategoryType.Newbie, true, false),
                new("GlazedMelon", FoodCategoryType.Newbie, true, false),
                new("MelonBlueberrySorbet", FoodCategoryType.Newbie, false, false),
                new("PickledMelon", FoodCategoryType.Newbie, true, false),
                new("PumpkinMelonCake", FoodCategoryType.Newbie, false, false),
                new("SpicyTofu", FoodCategoryType.Newbie, true, false),
                new("RedSalad", FoodCategoryType.Newbie, true, false),
                new("RedSaladOmelet", FoodCategoryType.Newbie, true, false),
                new("CornSoup", FoodCategoryType.Newbie, true, false),
                new("Burrito", FoodCategoryType.Newbie, true, false),
                new("CornSalad", FoodCategoryType.Newbie, true, false),
                new("Harunosarada", FoodCategoryType.Newbie, false, false),
                new("SpaghettiSalad", FoodCategoryType.Newbie, true, false),
                new("AgeTofu", FoodCategoryType.Newbie, true, false),
                new("SpicySpaghetti", FoodCategoryType.Newbie, true, false)
            };

            foreach (var command in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(command);

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

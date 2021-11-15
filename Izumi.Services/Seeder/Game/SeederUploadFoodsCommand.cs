using System.Threading;
using System.Threading.Tasks;
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
                new("Tortilla", true, false),
                new("Bread", true, false),
                new("FriedEgg", false, false),
                new("BeanHotpot", true, false),
                new("Spaghetti", true, false),
                new("VegetableMedley", true, false),
                new("EggplantParmesan", true, false),
                new("Pancakes", true, false),
                new("Bruschetta", true, false),
                new("Hashbrowns", true, false),
                new("RedPlate", true, false),
                new("Omelet", true, false),
                new("PepperPoppers", true, false),
                new("GarlicOil", true, false),
                new("RadishSalad", true, false),
                new("FarmBreakfast", true, false),
                new("SuperMeal", true, false),
                new("Pizza", true, false),
                new("ArtichokeDip", true, false),
                new("ParsnipSoup", true, false),
                new("CranberrySauce", true, false),
                new("CheeseCauliflower", true, false),
                new("Coleslaw", true, false),
                new("Cookie", true, false),
                new("ChocolateCake", false, false),
                new("IceCream", true, false),
                new("GlazedYams", true, false),
                new("BlueberryTart", true, false),
                new("PumpkinSoup", true, false),
                new("AutumnTale", true, false),
                new("CompleteBreakfast", true, false),
                new("RhubarbPie", true, false),
                new("PinkCake", true, false),
                new("PumpkinPie", true, false),
                new("Onigiri", false, false),
                new("CurryRice", true, false),
                new("Oyakodon", false, false),
                new("ParsnipChips", true, false),
                new("GarlicRice", true, false),
                new("HerbsEggs", true, false),
                new("SpringSalad", true, false),
                new("CreamSoup", true, false),
                new("BakedGarlic", true, false),
                new("RiceBalls", true, false),
                new("CreamyPuree", true, false),
                new("MochiCakes", true, false),
                new("HokkaidoBuns", false, false),
                new("FruitIce", true, false),
                new("ChakinSibori", false, false),
                new("GlazedParsnip", true, false),
                new("StrawberryCake", false, false),
                new("StrawberryRhubarbJam", false, false),
                new("Tofu", true, false),
                new("GlazedMelon", true, false),
                new("MelonBlueberrySorbet", false, false),
                new("PickledMelon", true, false),
                new("PumpkinMelonCake", false, false),
                new("SpicyTofu", true, false),
                new("RedSalad", true, false),
                new("RedSaladOmelet", true, false),
                new("CornSoup", true, false),
                new("Burrito", true, false),
                new("CornSalad", true, false),
                new("Harunosarada", false, false),
                new("SpaghettiSalad", true, false),
                new("AgeTofu", true, false),
                new("SpicySpaghetti", true, false),
                new("SpecialPumpkinPie", false, true),
                new("SpecialEggplantParmesan", false, true)
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

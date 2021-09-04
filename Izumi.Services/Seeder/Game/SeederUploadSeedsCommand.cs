using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Seed.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadSeedsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadSeedsHandler : IRequestHandler<SeederUploadSeedsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadSeedsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadSeedsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateSeedCommand[]
            {
                new("GreenBeanSeeds", SeasonType.Spring, 3, 2, false, 60),
                new("PotatoSeeds", SeasonType.Spring, 3, 0, true, 50),
                new("StrawberrySeeds", SeasonType.Spring, 3, 2, true, 30),
                new("KaleSeeds", SeasonType.Spring, 3, 0, false, 70),
                new("ParsnipSeeds", SeasonType.Spring, 3, 0, false, 20),
                new("RhubarbSeeds", SeasonType.Spring, 4, 0, false, 100),
                new("CauliflowerSeeds", SeasonType.Spring, 3, 0, false, 80),
                new("GarlicSeeds", SeasonType.Spring, 3, 0, false, 40),
                new("MelonSeeds", SeasonType.Summer, 4, 0, false, 80),
                new("HotPepperSeeds", SeasonType.Summer, 3, 2, true, 40),
                new("RedCabbageSeeds", SeasonType.Summer, 4, 0, false, 100),
                new("CornSeeds", SeasonType.Summer, 4, 2, false, 150),
                new("TomatoSeeds", SeasonType.Summer, 3, 2, true, 50),
                new("WheatSeeds", SeasonType.Summer, 3, 0, false, 10),
                new("RadishSeeds", SeasonType.Summer, 4, 0, false, 40),
                new("HopsSeeds", SeasonType.Summer, 3, 1, false, 60),
                new("BlueberrySeeds", SeasonType.Summer, 4, 2, true, 80),
                new("AmaranthSeeds", SeasonType.Autumn, 3, 0, false, 70),
                new("ArtichokeSeeds", SeasonType.Autumn, 3, 0, false, 30),
                new("EggplantSeeds", SeasonType.Autumn, 2, 3, false, 20),
                new("YamSeeds", SeasonType.Autumn, 3, 0, true, 60),
                new("BokChoySeeds", SeasonType.Autumn, 3, 0, false, 50),
                new("GrapeSeeds", SeasonType.Autumn, 3, 2, false, 60),
                new("CranberrySeeds", SeasonType.Autumn, 3, 0, true, 240),
                new("BeetSeeds", SeasonType.Autumn, 3, 0, false, 20),
                new("PumpkinSeeds", SeasonType.Autumn, 3, 0, false, 100),
                new("SunflowerSeeds", SeasonType.Summer, 2, 0, true, 50),
                new("RiceSeeds", SeasonType.Spring, 2, 0, false, 75),
                new("CoffeeBeanSeeds", SeasonType.Spring, 4, 2, true, 700)
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

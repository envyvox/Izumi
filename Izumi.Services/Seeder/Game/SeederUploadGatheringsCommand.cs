using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Gathering.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadGatheringsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadGatheringsHandler
        : IRequestHandler<SeederUploadGatheringsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadGatheringsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadGatheringsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateGatheringCommand[]
            {
                new("Grass", LocationType.ExploreGarden, 2),
                new("Wood", LocationType.ExploreGarden, 5),
                new("Flax", LocationType.ExploreGarden, 15),
                new("Resin", LocationType.ExploreGarden, 3),
                new("Mushroom", LocationType.ExploreGarden, 20),
                new("Stone", LocationType.ExploreCastle, 2),
                new("Iron", LocationType.ExploreCastle, 10),
                new("Gold", LocationType.ExploreCastle, 25),
                new("Coal", LocationType.ExploreCastle, 5)
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

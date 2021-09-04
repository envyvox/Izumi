using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Game.Crop.Commands;
using Izumi.Services.Game.Seed.Queries;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadCropsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadCropsHandler : IRequestHandler<SeederUploadCropsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadCropsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadCropsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var seeds = await _mediator.Send(new GetSeedsQuery());

            foreach (var seed in seeds)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(new CreateCropCommand(seed.Name.Replace("Seeds", ""), 999, seed.Id));

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

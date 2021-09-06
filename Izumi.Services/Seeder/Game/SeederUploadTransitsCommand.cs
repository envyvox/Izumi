using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Transit.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadTransitsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadTransitsHandler : IRequestHandler<SeederUploadTransitsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadTransitsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadTransitsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var locations = new List<LocationType>
            {
                LocationType.Capital,
                LocationType.Garden,
                LocationType.Seaport,
                LocationType.Castle,
                LocationType.Village
            };

            foreach (var departure in locations)
            {
                foreach (var destination in locations.Where(x => x != departure))
                {
                    result.Total++;

                    try
                    {
                        await _mediator.Send(new CreateTransitCommand(
                            departure, destination, TimeSpan.FromMinutes(5), 1));

                        result.Affected++;
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return result;
        }
    }
}

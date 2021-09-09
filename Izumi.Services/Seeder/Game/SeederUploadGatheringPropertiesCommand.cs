using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Gathering.Commands;
using Izumi.Services.Game.Gathering.Queries;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadGatheringPropertiesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadGatheringPropertiesHandler
        : IRequestHandler<SeederUploadGatheringPropertiesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadGatheringPropertiesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadGatheringPropertiesCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var gatherings = await _mediator.Send(new GetGatheringsQuery());
            var properties = Enum
                .GetValues(typeof(GatheringPropertyType))
                .Cast<GatheringPropertyType>()
                .ToArray();

            foreach (var gathering in gatherings)
            {
                foreach (var property in properties)
                {
                    result.Total++;

                    try
                    {
                        await _mediator.Send(new CreateGatheringPropertyCommand(gathering.Id, property, 1));

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

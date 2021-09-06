using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.World.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadWorldPropertiesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadWorldPropertiesHandler
        : IRequestHandler<SeederUploadWorldPropertiesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadWorldPropertiesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadWorldPropertiesCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();

            foreach (var type in Enum.GetValues(typeof(WorldPropertyType)).Cast<WorldPropertyType>())
            {
                result.Total++;

                try
                {
                    await _mediator.Send(new CreateWorldPropertyCommand(type, 1));

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

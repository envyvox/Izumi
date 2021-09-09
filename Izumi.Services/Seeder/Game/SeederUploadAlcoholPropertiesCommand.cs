using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Alcohol.Commands;
using Izumi.Services.Game.Alcohol.Queries;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadAlcoholPropertiesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadAlcoholPropertiesHandler
        : IRequestHandler<SeederUploadAlcoholPropertiesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadAlcoholPropertiesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadAlcoholPropertiesCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var alcohols = await _mediator.Send(new GetAlcoholsQuery());
            var properties = Enum
                .GetValues(typeof(AlcoholPropertyType))
                .Cast<AlcoholPropertyType>()
                .ToArray();

            foreach (var alcohol in alcohols)
            {
                foreach (var property in properties)
                {
                    result.Total++;

                    try
                    {
                        await _mediator.Send(new CreateAlcoholPropertyCommand(alcohol.Id, property, 1));

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

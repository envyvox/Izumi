using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Crafting.Commands;
using Izumi.Services.Game.Crafting.Queries;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadCraftingPropertiesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadCraftingPropertiesHandler
        : IRequestHandler<SeederUploadCraftingPropertiesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadCraftingPropertiesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadCraftingPropertiesCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var craftings = await _mediator.Send(new GetCraftingsQuery());
            var properties = Enum
                .GetValues(typeof(CraftingPropertyType))
                .Cast<CraftingPropertyType>()
                .ToArray();

            foreach (var crafting in craftings)
            {
                foreach (var property in properties)
                {
                    result.Total++;

                    try
                    {
                        await _mediator.Send(new CreateCraftingPropertyCommand(crafting.Id, property, 1));

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

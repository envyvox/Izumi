using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Game.Crafting.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadCraftingsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadCraftingsHandler : IRequestHandler<SeederUploadCraftingsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadCraftingsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadCraftingsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateCraftingCommand[]
            {
                new("Stick"),
                new("Board"),
                new("IronBar"),
                new("GoldBar"),
                new("Cloth"),
                new("Rope"),
                new("WheatFlour"),
                new("Oil"),
                new("Vinegar"),
                new("Sugar"),
                new("Mayonnaise"),
                new("Cheese")
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

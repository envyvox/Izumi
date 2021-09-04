using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Game.Drink.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadDrinksCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadDrinksHandler : IRequestHandler<SeederUploadDrinksCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadDrinksHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadDrinksCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateDrinkCommand[]
            {
                new("Coffee")
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

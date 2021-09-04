using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Game.Alcohol.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadAlcoholsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadAlcoholsHandler : IRequestHandler<SeederUploadAlcoholsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadAlcoholsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadAlcoholsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateAlcoholCommand[]
            {
                new("Beer"),
                new("BlueberryBeer"),
                new("Sake"),
                new("Wine")
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

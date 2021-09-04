using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Game.Seafood.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadSeafoodsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadSeafoodsHandler : IRequestHandler<SeederUploadSeafoodsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadSeafoodsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadSeafoodsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateSeafoodCommand[]
            {
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

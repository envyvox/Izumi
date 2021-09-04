using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Util;
using Izumi.Services.Game.Product.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadProductsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadProductsHandler : IRequestHandler<SeederUploadProductsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadProductsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadProductsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateProductCommand[]
            {
                new("Egg", 72),
                new("Product", 157)
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

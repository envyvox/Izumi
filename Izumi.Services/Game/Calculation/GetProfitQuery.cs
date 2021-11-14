using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Calculation
{
    public record GetProfitQuery(uint CostPrice, uint CraftingPrice, uint MarketPrice) : IRequest<uint>;

    public class GetProfitHandler : IRequestHandler<GetProfitQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetProfitHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetProfitQuery request, CancellationToken ct)
        {
            var marketTaxPercent = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.MarketTaxPercent));

            return (uint) (request.MarketPrice - request.MarketPrice / 100.0 * marketTaxPercent -
                           (request.CostPrice + request.CraftingPrice));
        }
    }
}
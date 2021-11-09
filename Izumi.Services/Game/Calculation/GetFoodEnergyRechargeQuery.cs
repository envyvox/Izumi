using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Calculation
{
    public record GetFoodEnergyRechargeQuery(uint CostPrice, uint CraftingPrice) : IRequest<uint>;

    public class GetFoodEnergyRechargeHandler : IRequestHandler<GetFoodEnergyRechargeQuery, uint>
    {
        private readonly IMediator _mediator;

        public GetFoodEnergyRechargeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<uint> Handle(GetFoodEnergyRechargeQuery request, CancellationToken ct)
        {
            var foodEnergyPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EconomyFoodEnergyPrice));

            return (request.CostPrice + request.CraftingPrice) / foodEnergyPrice + 2;
        }
    }
}
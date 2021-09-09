using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Fish.Queries
{
    public record CheckFishingSuccessQuery(FishRarityType Rarity) : IRequest<bool>;

    public class CheckFishingSuccessHandler : IRequestHandler<CheckFishingSuccessQuery, bool>
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public CheckFishingSuccessHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(CheckFishingSuccessQuery request, CancellationToken ct)
        {
            var chance = await _mediator.Send(new GetWorldPropertyValueQuery(request.Rarity switch
            {
                FishRarityType.Common => WorldPropertyType.FishFailChanceCommon,
                FishRarityType.Rare => WorldPropertyType.FishFailChanceRare,
                FishRarityType.Epic => WorldPropertyType.FishFailChanceEpic,
                FishRarityType.Mythical => WorldPropertyType.FishFailChanceMythical,
                FishRarityType.Legendary => WorldPropertyType.FishFailChanceLegendary,
                _ => throw new ArgumentOutOfRangeException()
            }));

            return _random.Next(1, 101) > chance;
        }
    }
}

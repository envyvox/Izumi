using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Game.Fish.Queries
{
    public record GetRandomFishRarityQuery : IRequest<FishRarityType>;

    public class GetRandomFishRarityHandler : IRequestHandler<GetRandomFishRarityQuery, FishRarityType>
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public GetRandomFishRarityHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<FishRarityType> Handle(GetRandomFishRarityQuery request, CancellationToken ct)
        {
            while (true)
            {
                var rand = _random.Next(1, 101);
                long current = 0;

                foreach (var rarity in Enum.GetValues(typeof(FishRarityType)).Cast<FishRarityType>())
                {
                    var chance = await _mediator.Send(new GetWorldPropertyValueQuery(rarity switch
                    {
                        FishRarityType.Common => WorldPropertyType.FishRarityChanceCommon,
                        FishRarityType.Rare => WorldPropertyType.FishRarityChanceRare,
                        FishRarityType.Epic => WorldPropertyType.FishRarityChanceEpic,
                        FishRarityType.Mythical => WorldPropertyType.FishRarityChanceMythical,
                        FishRarityType.Legendary => WorldPropertyType.FishRarityChanceLegendary,
                        _ => throw new ArgumentOutOfRangeException()
                    }));

                    if (current <= rand && rand < current + chance) return rarity;

                    current += chance;
                }
            }
        }
    }
}

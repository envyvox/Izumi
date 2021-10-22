using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Fish.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadFishesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadFishesHandler : IRequestHandler<SeederUploadFishesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadFishesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadFishesCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateFishCommand[]
            {
                new("Carp", FishRarityType.Common, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 20),
                new("Bream", FishRarityType.Common, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Night, 23),
                new("Ghostfish", FishRarityType.Common, new List<SeasonType> { SeasonType.Summer, SeasonType.Autumn }, WeatherType.Any, TimesDayType.Any, 20),
                new("Chub", FishRarityType.Common, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 20),
                new("Sandfish", FishRarityType.Rare, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Day, 45),
                new("Bullhead", FishRarityType.Rare, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 40),
                new("Woodskip", FishRarityType.Rare, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 40),
                new("LargemouthBass", FishRarityType.Epic, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Day, 90),
                new("Slimejack", FishRarityType.Epic, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 80),
                new("ScorpionCarp", FishRarityType.Epic, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Day, 90),
                new("VoidSalmon", FishRarityType.Epic, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 80),
                new("Stonefish", FishRarityType.Mythical, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 160),
                new("IcePip", FishRarityType.Mythical, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 160),
                new("LavaEel", FishRarityType.Mythical, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 160),
                new("MutantCarp", FishRarityType.Legendary, new List<SeasonType> { SeasonType.Any }, WeatherType.Any, TimesDayType.Any, 320),
                new("Sunfish", FishRarityType.Common, new List<SeasonType> { SeasonType.Spring, SeasonType.Summer }, WeatherType.Clear, TimesDayType.Day, 38),
                new("Catfish", FishRarityType.Mythical, new List<SeasonType> { SeasonType.Spring, SeasonType.Autumn, SeasonType.Winter }, WeatherType.Rain, TimesDayType.Any, 230),
                new("Herring", FishRarityType.Common, new List<SeasonType> { SeasonType.Spring, SeasonType.Winter }, WeatherType.Any, TimesDayType.Any, 30),
                new("MidnightCarp", FishRarityType.Epic, new List<SeasonType> { SeasonType.Autumn, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 130),
                new("Salmon", FishRarityType.Rare, new List<SeasonType> { SeasonType.Autumn }, WeatherType.Any, TimesDayType.Day, 75),
                new("Sardine", FishRarityType.Common, new List<SeasonType> { SeasonType.Spring, SeasonType.Autumn, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 28),
                new("SmallmouthBass", FishRarityType.Common, new List<SeasonType> { SeasonType.Spring, SeasonType.Autumn }, WeatherType.Any, TimesDayType.Any, 30),
                new("Tilapia", FishRarityType.Rare, new List<SeasonType> { SeasonType.Summer, SeasonType.Autumn }, WeatherType.Any, TimesDayType.Day, 65),
                new("RedMullet", FishRarityType.Rare, new List<SeasonType> { SeasonType.Summer, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 65),
                new("Pike", FishRarityType.Epic, new List<SeasonType> { SeasonType.Summer, SeasonType.Winter }, WeatherType.Any, TimesDayType.Any, 120),
                new("Putterfish", FishRarityType.Mythical, new List<SeasonType> { SeasonType.Summer }, WeatherType.Clear, TimesDayType.Day, 312),
                new("Octopus", FishRarityType.Epic, new List<SeasonType> { SeasonType.Summer }, WeatherType.Any, TimesDayType.Day, 150),
                new("RainbowTrout", FishRarityType.Rare, new List<SeasonType> { SeasonType.Summer }, WeatherType.Clear, TimesDayType.Day, 85),
                new("Eel", FishRarityType.Rare, new List<SeasonType> { SeasonType.Spring, SeasonType.Autumn }, WeatherType.Rain, TimesDayType.Night, 75),
                new("Crimsonfish", FishRarityType.Legendary, new List<SeasonType> { SeasonType.Summer }, WeatherType.Any, TimesDayType.Any, 520),
                new("Squid", FishRarityType.Rare, new List<SeasonType> { SeasonType.Winter }, WeatherType.Any, TimesDayType.Night, 75),
                new("TigerTrout", FishRarityType.Epic, new List<SeasonType> { SeasonType.Autumn, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 130),
                new("Halibut", FishRarityType.Rare, new List<SeasonType> { SeasonType.Spring, SeasonType.Summer, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 55),
                new("SeaCucumber", FishRarityType.Rare, new List<SeasonType> { SeasonType.Autumn, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 65),
                new("Lingcod", FishRarityType.Epic, new List<SeasonType> { SeasonType.Winter }, WeatherType.Any, TimesDayType.Any, 140),
                new("SuperCucumber", FishRarityType.Mythical, new List<SeasonType> { SeasonType.Summer, SeasonType.Autumn }, WeatherType.Any, TimesDayType.Night, 252),
                new("Legend", FishRarityType.Legendary, new List<SeasonType> { SeasonType.Spring }, WeatherType.Rain, TimesDayType.Any, 590),
                new("Dorado", FishRarityType.Epic, new List<SeasonType> { SeasonType.Summer }, WeatherType.Any, TimesDayType.Day, 150),
                new("Tuna", FishRarityType.Epic, new List<SeasonType> { SeasonType.Summer, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 130),
                new("Glacierfish", FishRarityType.Legendary, new List<SeasonType> { SeasonType.Winter }, WeatherType.Any, TimesDayType.Any, 520),
                new("Flounder", FishRarityType.Epic, new List<SeasonType> { SeasonType.Spring, SeasonType.Summer }, WeatherType.Any, TimesDayType.Day, 130),
                new("Angler", FishRarityType.Legendary, new List<SeasonType> { SeasonType.Autumn }, WeatherType.Any, TimesDayType.Any, 520),
                new("Sturgeon", FishRarityType.Mythical, new List<SeasonType> { SeasonType.Summer, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 252),
                new("Albacore", FishRarityType.Rare, new List<SeasonType> { SeasonType.Autumn, SeasonType.Winter }, WeatherType.Any, TimesDayType.Day, 65),
                new("RedSnapper", FishRarityType.Common, new List<SeasonType> { SeasonType.Summer, SeasonType.Autumn }, WeatherType.Rain, TimesDayType.Day, 38),
                new("Anchovy", FishRarityType.Common, new List<SeasonType> { SeasonType.Spring, SeasonType.Autumn }, WeatherType.Any, TimesDayType.Any, 30),
                new("Walleye", FishRarityType.Epic, new List<SeasonType> { SeasonType.Autumn }, WeatherType.Rain, TimesDayType.Night, 170),
                new("Perch", FishRarityType.Rare, new List<SeasonType> { SeasonType.Winter }, WeatherType.Any, TimesDayType.Any, 70),
                new("Shad", FishRarityType.Rare, new List<SeasonType> { SeasonType.Spring, SeasonType.Summer, SeasonType.Autumn }, WeatherType.Rain, TimesDayType.Night, 65)
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

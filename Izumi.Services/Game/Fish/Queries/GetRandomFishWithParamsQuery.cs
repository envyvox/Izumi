using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Fish.Queries
{
    public record GetRandomFishWithParamsQuery(
            FishRarityType Rarity,
            TimesDayType TimesDay,
            WeatherType Weather,
            SeasonType Season)
        : IRequest<FishDto>;

    public class GetRandomFishWithParamsHandler : IRequestHandler<GetRandomFishWithParamsQuery, FishDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetRandomFishWithParamsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FishDto> Handle(GetRandomFishWithParamsQuery request, CancellationToken ct)
        {
            var entities = await _db.Fishes
                .OrderByRandom()
                .ToListAsync();

            var entity = entities.FirstOrDefault(x =>
                x.Rarity == request.Rarity &&
                (x.CatchTimesDay == TimesDayType.Any || x.CatchTimesDay == request.TimesDay) &&
                (x.CatchWeather == WeatherType.Any || x.CatchWeather == request.Weather) &&
                (x.CatchSeasons.Contains(SeasonType.Any) || x.CatchSeasons.Contains(request.Season)));

            if (entity is null)
            {
                throw new Exception(
                    "fish with params: " +
                    $"rarity {request.Rarity.ToString()}, " +
                    $"timesDay {request.TimesDay.ToString()}, " +
                    $"weather {request.Weather.ToString()}, " +
                    $"season {request.Season.ToString()}" +
                    "not found");
            }

            return _mapper.Map<FishDto>(entity);
        }
    }
}

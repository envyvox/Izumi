using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Fish.Commands
{
    public record UpdateFishCommand(
            Guid Id,
            string Name,
            FishRarityType Rarity,
            TimesDayType CatchTimesDay,
            WeatherType CatchWeather,
            List<SeasonType> CatchSeasons,
            uint Price)
        : IRequest<FishDto>;

    public class UpdateFishHandler : IRequestHandler<UpdateFishCommand, FishDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateFishHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FishDto> Handle(UpdateFishCommand request, CancellationToken ct)
        {
            var entity = await _db.Fishes
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.Rarity = request.Rarity;
            entity.CatchTimesDay = request.CatchTimesDay;
            entity.CatchWeather = request.CatchWeather;
            entity.CatchSeasons = request.CatchSeasons;
            entity.Price = request.Price;

            await _db.UpdateEntity(entity);

            return _mapper.Map<FishDto>(entity);
        }
    }
}

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
    public record CreateFishCommand(
            string Name,
            FishRarityType Rarity,
            WeatherType CatchWeather,
            TimesDayType CatchTimesDay,
            List<SeasonType> CatchSeasons,
            uint Price)
        : IRequest<FishDto>;

    public class CreateFishHandler : IRequestHandler<CreateFishCommand, FishDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CreateFishHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FishDto> Handle(CreateFishCommand request, CancellationToken ct)
        {
            var exist = await _db.Fishes
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"fish with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Fish
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            });

            return _mapper.Map<FishDto>(created);
        }
    }
}

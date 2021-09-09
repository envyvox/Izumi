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
    public record GetRandomFishWithRarityQuery(FishRarityType Rarity) : IRequest<FishDto>;

    public class GetRandomFishWithRarityHandler : IRequestHandler<GetRandomFishWithRarityQuery, FishDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetRandomFishWithRarityHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FishDto> Handle(GetRandomFishWithRarityQuery request, CancellationToken ct)
        {
            var entity = await _db.Fishes
                .OrderByRandom()
                .Where(x => x.Rarity == request.Rarity)
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new Exception($"fish with rarity {request.Rarity.ToString()} not found");
            }

            return _mapper.Map<FishDto>(entity);
        }
    }
}

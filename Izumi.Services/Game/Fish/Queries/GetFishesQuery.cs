using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Fish.Queries
{
    public record GetFishesQuery : IRequest<List<FishDto>>;

    public class GetFishesHandler : IRequestHandler<GetFishesQuery, List<FishDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetFishesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<FishDto>> Handle(GetFishesQuery request, CancellationToken ct)
        {
            var entities = await _db.Fishes
                .ToListAsync();

            return _mapper.Map<List<FishDto>>(entities);
        }
    }
}

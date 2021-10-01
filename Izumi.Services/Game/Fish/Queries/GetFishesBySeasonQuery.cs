using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Fish.Queries
{
    public record GetFishesBySeasonQuery(SeasonType Season) : IRequest<List<FishDto>>;

    public class GetFishesBySeasonHandler : IRequestHandler<GetFishesBySeasonQuery, List<FishDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetFishesBySeasonHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<FishDto>> Handle(GetFishesBySeasonQuery request, CancellationToken ct)
        {
            var entities = await _db.Fishes
                .AsQueryable()
                .OrderBy(x => x.AutoIncrementedId)
                .ToListAsync();

            var filteredEntities = entities
                .Where(x =>
                    x.CatchSeasons.Contains(SeasonType.Any) ||
                    x.CatchSeasons.Contains(request.Season))
                .ToList();

            return _mapper.Map<List<FishDto>>(filteredEntities);
        }
    }
}

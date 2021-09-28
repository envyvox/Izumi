using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Building.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Building.Queries
{
    public record GetBuildingsQuery : IRequest<List<BuildingDto>>;

    public class GetBuildingsHandler : IRequestHandler<GetBuildingsQuery, List<BuildingDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetBuildingsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<BuildingDto>> Handle(GetBuildingsQuery request, CancellationToken ct)
        {
            var entities = await _db.Buildings
                .Include(x => x.Ingredients)
                .ToListAsync();

            return _mapper.Map<List<BuildingDto>>(entities);
        }
    }
}

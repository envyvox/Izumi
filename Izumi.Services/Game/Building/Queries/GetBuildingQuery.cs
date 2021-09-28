using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Building.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Building.Queries
{
    public record GetBuildingQuery(BuildingType Type) : IRequest<BuildingDto>;

    public class GetBuildingHandler : IRequestHandler<GetBuildingQuery, BuildingDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetBuildingHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<BuildingDto> Handle(GetBuildingQuery request, CancellationToken ct)
        {
            var entity = await _db.Buildings
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception($"building with type {request.Type.ToString()} not found");
            }

            return _mapper.Map<BuildingDto>(entity);
        }
    }
}

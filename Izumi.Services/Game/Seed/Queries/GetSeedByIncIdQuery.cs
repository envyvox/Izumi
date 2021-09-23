using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seed.Queries
{
    public record GetSeedByIncIdQuery(long IncId) : IRequest<SeedDto>;

    public class GetSeedByIncIdHandler : IRequestHandler<GetSeedByIncIdQuery, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetSeedByIncIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<SeedDto> Handle(GetSeedByIncIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Seeds
                .Include(x => x.Crop)
                .SingleOrDefaultAsync(x => x.AutoIncrementedId == request.IncId);

            if (entity is null)
            {
                throw new Exception("никогда не слышала о семенах с таким номером.");
            }

            return _mapper.Map<SeedDto>(entity);
        }
    }
}

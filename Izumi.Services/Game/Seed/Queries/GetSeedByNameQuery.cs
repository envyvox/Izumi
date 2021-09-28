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
    public record GetSeedByNameQuery(string Name) : IRequest<SeedDto>;

    public class GetSeedByNameHandler : IRequestHandler<GetSeedByNameQuery, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetSeedByNameHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<SeedDto> Handle(GetSeedByNameQuery request, CancellationToken ct)
        {
            var entity = await _db.Seeds
                .Include(x => x.Crop)
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                throw new Exception($"seed with name {request.Name} not found");
            }

            return _mapper.Map<SeedDto>(entity);
        }
    }
}

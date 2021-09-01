using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seed.Queries
{
    public record GetSeedsQuery : IRequest<List<SeedDto>>;

    public class GetSeedsHandler : IRequestHandler<GetSeedsQuery, List<SeedDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetSeedsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<SeedDto>> Handle(GetSeedsQuery request, CancellationToken ct)
        {
            var entities = await _db.Seeds
                .ToListAsync();

            return _mapper.Map<List<SeedDto>>(entities);
        }
    }
}

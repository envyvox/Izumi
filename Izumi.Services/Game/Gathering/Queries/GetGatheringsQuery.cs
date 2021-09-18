using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Gathering.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Queries
{
    public record GetGatheringsQuery : IRequest<List<GatheringDto>>;

    public class GetGatheringsHandler : IRequestHandler<GetGatheringsQuery, List<GatheringDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetGatheringsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<GatheringDto>> Handle(GetGatheringsQuery request, CancellationToken ct)
        {
            var entities = await _db.Gatherings
                .Include(x => x.Properties)
                .OrderBy(x => x.AutoIncrementedId)
                .ToListAsync();

            return _mapper.Map<List<GatheringDto>>(entities);
        }
    }
}

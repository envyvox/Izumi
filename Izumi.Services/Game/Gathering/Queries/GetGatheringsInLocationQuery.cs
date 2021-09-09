using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Gathering.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Queries
{
    public record GetGatheringsInLocationQuery(LocationType Location) : IRequest<List<GatheringDto>>;

    public class GetGatheringsInLocationHandler : IRequestHandler<GetGatheringsInLocationQuery, List<GatheringDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetGatheringsInLocationHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<GatheringDto>> Handle(GetGatheringsInLocationQuery request, CancellationToken ct)
        {
            var entities = await _db.Gatherings
                .AsQueryable()
                .Where(x => x.Location == request.Location)
                .ToListAsync();

            return _mapper.Map<List<GatheringDto>>(entities);
        }
    }
}

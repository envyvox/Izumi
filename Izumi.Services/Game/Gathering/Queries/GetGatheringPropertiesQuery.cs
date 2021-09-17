using System;
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
    public record GetGatheringPropertiesQuery(Guid GatheringId) : IRequest<List<GatheringPropertyDto>>;

    public class GetGatheringPropertiesHandler
        : IRequestHandler<GetGatheringPropertiesQuery, List<GatheringPropertyDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetGatheringPropertiesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<GatheringPropertyDto>> Handle(GetGatheringPropertiesQuery request, CancellationToken ct)
        {
            var entities = await _db.GatheringProperties
                .Include(x => x.Gathering)
                .Where(x => x.GatheringId == request.GatheringId)
                .ToListAsync();

            return _mapper.Map<List<GatheringPropertyDto>>(entities);
        }
    }
}

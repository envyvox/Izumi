using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Gathering.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Queries
{
    public record GetGatheringQuery(Guid Id) : IRequest<GatheringDto>;

    public class GetGatheringHandler : IRequestHandler<GetGatheringQuery, GatheringDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetGatheringHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<GatheringDto> Handle(GetGatheringQuery request, CancellationToken ct)
        {
            var entity = await _db.Gatherings
                .Include(x => x.Properties)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"gathering {request.Id} not found");
            }

            return _mapper.Map<GatheringDto>(entity);
        }
    }
}

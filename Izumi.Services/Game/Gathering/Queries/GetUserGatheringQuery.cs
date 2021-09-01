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
    public record GetUserGatheringQuery(long UserId, Guid GatheringId) : IRequest<UserGatheringDto>;

    public class GetUserGatheringHandler : IRequestHandler<GetUserGatheringQuery, UserGatheringDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserGatheringHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserGatheringDto> Handle(GetUserGatheringQuery request, CancellationToken ct)
        {
            var entity = await _db.UserGatherings
                .Include(x => x.Gathering)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.GatheringId == request.GatheringId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have gathering {request.GatheringId}");
            }

            return _mapper.Map<UserGatheringDto>(entity);
        }
    }
}

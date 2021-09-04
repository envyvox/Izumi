using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Gathering.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Queries
{
    public record GetUserGatheringsQuery(long UserId) : IRequest<List<UserGatheringDto>>;

    public class GetUserGatheringsHandler : IRequestHandler<GetUserGatheringsQuery, List<UserGatheringDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserGatheringsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserGatheringDto>> Handle(GetUserGatheringsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserGatherings
                .AmountNotZero()
                .Include(x => x.Gathering)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserGatheringDto>>(entities);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Reputation.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Reputation.Queries
{
    public record GetUserReputationsQuery(long UserId) : IRequest<List<UserReputationDto>>;

    public class GetUserReputationsHandler : IRequestHandler<GetUserReputationsQuery, List<UserReputationDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserReputationsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserReputationDto>> Handle(GetUserReputationsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserReputations
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserReputationDto>>(entities);
        }
    }
}

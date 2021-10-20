using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Effect.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Effect.Queries
{
    public record GetUserEffectsQuery(long UserId) : IRequest<List<UserEffectDto>>;

    public class GetUserEffectsHandler : IRequestHandler<GetUserEffectsQuery, List<UserEffectDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserEffectsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserEffectDto>> Handle(GetUserEffectsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserEffects
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserEffectDto>>(entities);
        }
    }
}

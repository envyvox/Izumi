using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Effect.Queries
{
    public record GetUsersWithEffectCountQuery(EffectType Type) : IRequest<uint>;

    public class GetUsersWithEffectCountHandler : IRequestHandler<GetUsersWithEffectCountQuery, uint>
    {
        private readonly AppDbContext _db;

        public GetUsersWithEffectCountHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<uint> Handle(GetUsersWithEffectCountQuery request, CancellationToken ct)
        {
            return (uint) await _db.UserEffects
                .CountAsync(x => x.Type == request.Type);
        }
    }
}

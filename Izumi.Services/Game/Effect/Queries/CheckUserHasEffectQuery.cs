using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Effect.Queries
{
    public record CheckUserHasEffectQuery(long UserId, EffectType Type) : IRequest<bool>;

    public class CheckUserHasEffectHandler : IRequestHandler<CheckUserHasEffectQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserHasEffectHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserHasEffectQuery request, CancellationToken ct)
        {
            return await _db.UserEffects
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);
        }
    }
}

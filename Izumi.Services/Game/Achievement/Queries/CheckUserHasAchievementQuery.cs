using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Achievement.Queries
{
    public record CheckUserHasAchievementQuery(long UserId, AchievementType Type) : IRequest<bool>;

    public class CheckUserHasAchievementHandler : IRequestHandler<CheckUserHasAchievementQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserHasAchievementHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserHasAchievementQuery request, CancellationToken ct)
        {
            return await _db.UserAchievements
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);
        }
    }
}

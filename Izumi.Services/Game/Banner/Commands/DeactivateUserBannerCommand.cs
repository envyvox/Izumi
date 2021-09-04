using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Commands
{
    public record DeactivateUserBannerCommand(long UserId, Guid BannerId) : IRequest;

    public class DeactivateUserBannerHandler : IRequestHandler<DeactivateUserBannerCommand>
    {
        private readonly AppDbContext _db;

        public DeactivateUserBannerHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(DeactivateUserBannerCommand request, CancellationToken ct)
        {
            var entity = await _db.UserBanners
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.BannerId == request.BannerId);

            entity.IsActive = false;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}

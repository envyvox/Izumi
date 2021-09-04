using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Commands
{
    public record ActivateUserBannerCommand(long UserId, Guid BannerId) : IRequest;

    public class ActivateUserBannerHandler : IRequestHandler<ActivateUserBannerCommand>
    {
        private readonly AppDbContext _db;

        public ActivateUserBannerHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(ActivateUserBannerCommand request, CancellationToken ct)
        {
            var entity = await _db.UserBanners
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.BannerId == request.BannerId);

            entity.IsActive = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}

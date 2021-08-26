using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Commands
{
    public record AddBannerToUserCommand(long UserId, Guid BannerId, bool IsActive = false) : IRequest;

    public class AddBannerToUserHandler : IRequestHandler<AddBannerToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddBannerToUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddBannerToUserCommand request, CancellationToken ct)
        {
            var exist = await _db.UserBanners
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.BannerId == request.BannerId);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have banner {request.BannerId}");
            }

            await _db.CreateEntity(new UserBanner
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                BannerId = request.BannerId,
                IsActive = request.IsActive,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            return Unit.Value;
        }
    }
}

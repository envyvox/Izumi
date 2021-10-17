using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.Mute.Queries
{
    public record CheckUserMutedQuery(long UserId) : IRequest<bool>;

    public class CheckUserMutedHandler : IRequestHandler<CheckUserMutedQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserMutedHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserMutedQuery request, CancellationToken cancellationToken)
        {
            var timeNow = DateTimeOffset.UtcNow;
            return await _db.UserMutes
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Expiration > timeNow);
        }
    }
}

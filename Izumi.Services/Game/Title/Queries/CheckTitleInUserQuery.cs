using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Title.Queries
{
    public record CheckTitleInUserQuery(long UserId, TitleType Type) : IRequest<bool>;

    public class CheckTitleInUserHandler : IRequestHandler<CheckTitleInUserQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckTitleInUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckTitleInUserQuery request, CancellationToken ct)
        {
            return await _db.UserTitles
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);
        }
    }
}

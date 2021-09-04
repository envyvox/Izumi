using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Title.Queries
{
    public record GetUserTitlesQuery(long UserId) : IRequest<List<TitleType>>;

    public class GetUserTitlesHandler : IRequestHandler<GetUserTitlesQuery, List<TitleType>>
    {
        private readonly AppDbContext _db;

        public GetUserTitlesHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<List<TitleType>> Handle(GetUserTitlesQuery request, CancellationToken ct)
        {
            var entities = await _db.UserTitles
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.Type)
                .ToListAsync();

            return entities;
        }
    }
}

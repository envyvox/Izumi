using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Queries
{
    public record GetCurrentSeasonQuery : IRequest<SeasonType>;

    public class GetCurrentSeasonHandler : IRequestHandler<GetCurrentSeasonQuery, SeasonType>
    {
        private readonly AppDbContext _db;

        public GetCurrentSeasonHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<SeasonType> Handle(GetCurrentSeasonQuery request, CancellationToken ct)
        {
            return await _db.WorldSettings
                .AsQueryable()
                .Select(x => x.CurrentSeason)
                .SingleOrDefaultAsync();
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Building.Queries
{
    public record CheckUserHasBuildingQuery(long UserId, BuildingType BuildingType) : IRequest<bool>;

    public class CheckUserHasBuildingHandler : IRequestHandler<CheckUserHasBuildingQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserHasBuildingHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserHasBuildingQuery request, CancellationToken ct)
        {
            return await _db.UserBuildings
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.BuildingType == request.BuildingType);
        }
    }
}

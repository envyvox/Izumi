using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Commands
{
    public record UpdateCurrentSeasonCommand(SeasonType Season) : IRequest;

    public class UpdateCurrentSeasonHandler : IRequestHandler<UpdateCurrentSeasonCommand>
    {
        private readonly AppDbContext _db;

        public UpdateCurrentSeasonHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateCurrentSeasonCommand request, CancellationToken ct)
        {
            var entity = await _db.WorldSettings.SingleOrDefaultAsync();

            entity.CurrentSeason = request.Season;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}

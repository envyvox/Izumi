using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.World.Commands
{
    public record UpdateCurrentSeasonCommand(SeasonType Season) : IRequest;

    public class UpdateCurrentSeasonHandler : IRequestHandler<UpdateCurrentSeasonCommand>
    {
        private readonly ILogger<UpdateCurrentSeasonHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateCurrentSeasonHandler(
            DbContextOptions options,
            ILogger<UpdateCurrentSeasonHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateCurrentSeasonCommand request, CancellationToken ct)
        {
            var entity = await _db.WorldSettings.SingleOrDefaultAsync();

            entity.CurrentSeason = request.Season;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Updated current season to {Season}",
                request.Season.ToString());

            return Unit.Value;
        }
    }
}

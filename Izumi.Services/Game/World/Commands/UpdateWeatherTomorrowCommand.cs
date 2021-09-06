using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Commands
{
    public record UpdateWeatherTomorrowCommand(WeatherType WeatherTomorrow) : IRequest;

    public class UpdateWeatherTomorrowHandler : IRequestHandler<UpdateWeatherTomorrowCommand>
    {
        private readonly AppDbContext _db;

        public UpdateWeatherTomorrowHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateWeatherTomorrowCommand request, CancellationToken ct)
        {
            var entity = await _db.WorldSettings.SingleOrDefaultAsync();

            entity.WeatherTomorrow = request.WeatherTomorrow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}

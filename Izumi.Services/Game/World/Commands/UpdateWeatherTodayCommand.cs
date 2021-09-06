using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Commands
{
    public record UpdateWeatherTodayCommand(WeatherType WeatherToday) : IRequest;

    public class UpdateWeatherTodayHandler : IRequestHandler<UpdateWeatherTodayCommand>
    {
        private readonly AppDbContext _db;

        public UpdateWeatherTodayHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateWeatherTodayCommand request, CancellationToken ct)
        {
            var entity = await _db.WorldSettings.SingleOrDefaultAsync();

            entity.WeatherToday = request.WeatherToday;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}

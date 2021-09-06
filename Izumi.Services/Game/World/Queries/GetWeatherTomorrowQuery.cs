using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Queries
{
    public record GetWeatherTomorrowQuery : IRequest<WeatherType>;

    public class GetWeatherTomorrowHandler : IRequestHandler<GetWeatherTomorrowQuery, WeatherType>
    {
        private readonly AppDbContext _db;

        public GetWeatherTomorrowHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<WeatherType> Handle(GetWeatherTomorrowQuery request, CancellationToken ct)
        {
            return await _db.WorldSettings
                .AsQueryable()
                .Select(x => x.WeatherTomorrow)
                .SingleOrDefaultAsync();
        }
    }
}

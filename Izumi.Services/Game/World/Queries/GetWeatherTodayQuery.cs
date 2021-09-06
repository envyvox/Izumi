using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Queries
{
    public record GetWeatherTodayQuery : IRequest<WeatherType>;

    public class GetWeatherTodayHandler : IRequestHandler<GetWeatherTodayQuery, WeatherType>
    {
        private readonly AppDbContext _db;

        public GetWeatherTodayHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<WeatherType> Handle(GetWeatherTodayQuery request, CancellationToken ct)
        {
            return await _db.WorldSettings
                .AsQueryable()
                .Select(x => x.WeatherToday)
                .SingleOrDefaultAsync();
        }
    }
}

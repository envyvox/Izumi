using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Statistic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Statistic.Queries
{
    public record GetUserStatisticsQuery(long UserId) : IRequest<Dictionary<StatisticType, UserStatisticDto>>;

    public class GetUserStatisticsHandler
        : IRequestHandler<GetUserStatisticsQuery, Dictionary<StatisticType, UserStatisticDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserStatisticsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<Dictionary<StatisticType, UserStatisticDto>> Handle(GetUserStatisticsQuery request,
            CancellationToken ct)
        {
            var entities = await _db.UserStatistics
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToDictionaryAsync(x => x.Type);

            return _mapper.Map<Dictionary<StatisticType, UserStatisticDto>>(entities);
        }
    }
}

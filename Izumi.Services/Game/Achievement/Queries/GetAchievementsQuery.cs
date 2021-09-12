using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Achievement.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Achievement.Queries
{
    public record GetAchievementsQuery : IRequest<List<AchievementDto>>;

    public class GetAchievementsHandler : IRequestHandler<GetAchievementsQuery, List<AchievementDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetAchievementsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<AchievementDto>> Handle(GetAchievementsQuery request, CancellationToken ct)
        {
            var entities = await _db.Achievements
                .ToListAsync();

            return _mapper.Map<List<AchievementDto>>(entities);
        }
    }
}

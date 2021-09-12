using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Achievement.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Achievement.Queries
{
    public record GetAchievementsInCategoryQuery(AchievementCategoryType Category) : IRequest<List<AchievementDto>>;

    public class GetAchievementsInCategoryHandler
        : IRequestHandler<GetAchievementsInCategoryQuery, List<AchievementDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetAchievementsInCategoryHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<AchievementDto>> Handle(GetAchievementsInCategoryQuery request, CancellationToken ct)
        {
            var entities = await _db.Achievements
                .AsQueryable()
                .Where(x => x.Category == request.Category)
                .ToListAsync();

            return _mapper.Map<List<AchievementDto>>(entities);
        }
    }
}

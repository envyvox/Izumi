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
    public record GetUserAchievementsInCategoryQuery(
            long UserId,
            AchievementCategoryType Category)
        : IRequest<List<UserAchievementDto>>;

    public class GetUserAchievementsInCategoryHandler
        : IRequestHandler<GetUserAchievementsInCategoryQuery, List<UserAchievementDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserAchievementsInCategoryHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserAchievementDto>> Handle(GetUserAchievementsInCategoryQuery request,
            CancellationToken ct)
        {
            var entities = await _db.UserAchievements
                .Include(x => x.Achievement)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.Achievement.Category == request.Category)
                .ToListAsync();

            return _mapper.Map<List<UserAchievementDto>>(entities);
        }
    }
}

using System;
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
    public record GetAchievementQuery(AchievementType Type) : IRequest<AchievementDto>;

    public class GetAchievementHandler : IRequestHandler<GetAchievementQuery, AchievementDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetAchievementHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<AchievementDto> Handle(GetAchievementQuery request, CancellationToken ct)
        {
            var entity = await _db.Achievements
                .SingleOrDefaultAsync(x => x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception($"achievement {request.Type.ToString()} not found");
            }

            return _mapper.Map<AchievementDto>(entity);
        }
    }
}

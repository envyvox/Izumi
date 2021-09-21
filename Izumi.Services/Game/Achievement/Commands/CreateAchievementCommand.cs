using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Izumi.Services.Game.Achievement.Commands
{
    public record CreateAchievementCommand(
        AchievementType Type,
        AchievementCategoryType Category,
        string Name,
        AchievementRewardType RewardType,
        uint RewardNumber) : IRequest;

    public class CreateAchievementHandler : IRequestHandler<CreateAchievementCommand>
    {
        private readonly ILogger<CreateAchievementHandler> _logger;
        private readonly AppDbContext _db;

        public CreateAchievementHandler(
            DbContextOptions options,
            ILogger<CreateAchievementHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateAchievementCommand request, CancellationToken ct)
        {
            var exist = await _db.Achievements
                .AnyAsync(x => x.Type == request.Type);

            if (exist)
            {
                throw new Exception($"achievement {request.Type.ToString()} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Achievement
            {
                Type = request.Type,
                Category = request.Category,
                Name = request.Name,
                RewardType = request.RewardType,
                RewardNumber = request.RewardNumber
            });

            _logger.LogInformation(
                "Created achievement {Achievement}",
                JsonSerializer.Serialize(created));

            return Unit.Value;
        }
    }
}

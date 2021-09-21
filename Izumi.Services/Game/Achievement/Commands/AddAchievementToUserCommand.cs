using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Achievement.Commands
{
    public record AddAchievementToUserCommand(long UserId, AchievementType Type) : IRequest;

    public class AddAchievementToUserHandler : IRequestHandler<AddAchievementToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddAchievementToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddAchievementToUserHandler(
            DbContextOptions options,
            IMediator mediator,
            ILogger<AddAchievementToUserHandler> logger)
        {
            _db = new AppDbContext(options);
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddAchievementToUserCommand request, CancellationToken ct)
        {
            var exist = await _db.UserAchievements
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have achievement {request.Type.ToString()}");
            }

            await _db.CreateEntity(new UserAchievement
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Added achievement with type {Type} to user {UserId}",
                request.Type.ToString(), request.UserId);

            return await _mediator.Send(new AddAchievementRewardToUserCommand(request.UserId, request.Type));
        }
    }
}

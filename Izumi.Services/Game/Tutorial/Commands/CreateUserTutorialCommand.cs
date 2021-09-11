using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Tutorial.Commands
{
    public record CreateUserTutorialCommand(long UserId) : IRequest;

    public class CreateUserTutorialHandler : IRequestHandler<CreateUserTutorialCommand>
    {
        private readonly AppDbContext _db;

        public CreateUserTutorialHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateUserTutorialCommand request, CancellationToken ct)
        {
            var exist = await _db.UserTutorials
                .AnyAsync(x => x.UserId == request.UserId);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already started tutoril");
            }

            await _db.CreateEntity(new UserTutorial
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Step = TutorialStepType.CheckProfile,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            return Unit.Value;
        }
    }
}

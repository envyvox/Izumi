using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Box.Commands
{
    public record RemoveBoxFromUserCommand(long UserId, BoxType Box, uint Amount) : IRequest;

    public class RemoveBoxFromUserHandler : IRequestHandler<RemoveBoxFromUserCommand>
    {
        private readonly ILogger<RemoveBoxFromUserHandler> _logger;
        private readonly AppDbContext _db;

        public RemoveBoxFromUserHandler(
            DbContextOptions options,
            ILogger<RemoveBoxFromUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveBoxFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserBoxes
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Box == request.Box);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have box {request.Box}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} box {Box} amount {Amount}",
                request.UserId, request.Box.ToString(), request.Amount);

            return Unit.Value;
        }
    }
}

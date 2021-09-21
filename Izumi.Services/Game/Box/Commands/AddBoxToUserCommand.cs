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

namespace Izumi.Services.Game.Box.Commands
{
    public record AddBoxToUserCommand(long UserId, BoxType Box, uint Amount) : IRequest;

    public class AddBoxToUserHandler : IRequestHandler<AddBoxToUserCommand>
    {
        private readonly ILogger<AddBoxToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddBoxToUserHandler(
            DbContextOptions options,
            ILogger<AddBoxToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddBoxToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserBoxes
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Box == request.Box);

            if (entity is null)
            {
                await _db.CreateEntity(new UserBox
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Box = request.Box,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user box entity for user {UserId} with box {Box} and amount {Amount}",
                    request.UserId, request.Box.ToString(), request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} box {Box} amount {Amount}",
                    request.UserId, request.Box.ToString(), request.Amount);
            }

            return Unit.Value;
        }
    }
}

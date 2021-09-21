﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Statistic.Commands
{
    public record AddStatisticToUserCommand(long UserId, StatisticType Type, uint Amount = 1) : IRequest;

    public class AddStatisticToUserHandler : IRequestHandler<AddStatisticToUserCommand>
    {
        private readonly ILogger<AddStatisticToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddStatisticToUserHandler(
            DbContextOptions options,
            ILogger<AddStatisticToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddStatisticToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserStatistics
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                await _db.CreateEntity(new UserStatistic
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Type = request.Type,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user statistic entity for user {UserId} with type {Type} and amount {Amount}",
                    request.UserId, request.Type.ToString(), request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} statistic {Type} amount {Amount}",
                    request.UserId, request.Type.ToString(), request.Amount);
            }

            return Unit.Value;
        }
    }
}

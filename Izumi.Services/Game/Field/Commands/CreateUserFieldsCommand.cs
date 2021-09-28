using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Field.Commands
{
    public record CreateUserFieldsCommand(long UserId, IEnumerable<uint> Numbers) : IRequest;

    public class CreateUserFieldsHandler : IRequestHandler<CreateUserFieldsCommand>
    {
        private readonly ILogger<CreateUserFieldsHandler> _logger;
        private readonly AppDbContext _db;

        public CreateUserFieldsHandler(
            DbContextOptions options,
            ILogger<CreateUserFieldsHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserFieldsCommand request, CancellationToken ct)
        {
            foreach (var number in request.Numbers)
            {
                var exist = await _db.UserFields
                    .AnyAsync(x =>
                        x.UserId == request.UserId &&
                        x.Number == number);

                if (exist)
                {
                    throw new Exception($"user {request.UserId} already have field with number {number}");
                }

                await _db.CreateEntity(new UserField
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Number = number,
                    State = FieldStateType.Empty,
                    SeedId = null,
                    Progress = 0,
                    InReGrowth = false,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user field entity for user {UserId} with number {Number}",
                    request.UserId, number);
            }

            return Unit.Value;
        }
    }
}

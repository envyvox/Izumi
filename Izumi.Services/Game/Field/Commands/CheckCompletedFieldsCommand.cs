using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Field.Commands
{
    public record CheckCompletedFieldsCommand : IRequest;

    public class CheckCompletedFieldsHandler : IRequestHandler<CheckCompletedFieldsCommand>
    {
        private readonly ILogger<CheckCompletedFieldsHandler> _logger;
        private readonly AppDbContext _db;

        public CheckCompletedFieldsHandler(
            DbContextOptions options,
            ILogger<CheckCompletedFieldsHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(CheckCompletedFieldsCommand request, CancellationToken ct)
        {
            var entities = await _db.UserFields
                .AsQueryable()
                .Where(x =>
                    x.State != FieldStateType.Empty &&
                    x.State != FieldStateType.Completed &&
                    (x.InReGrowth == false && x.Progress >= x.Seed.GrowthDays ||
                     x.InReGrowth == true && x.Progress >= x.Seed.ReGrowthDays))
                .ToListAsync();

            foreach (var entity in entities)
            {
                entity.State = FieldStateType.Completed;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "User {UserId} field {FieldNumber} state marked as completed",
                    entity.UserId, entity.Number);
            }

            return Unit.Value;
        }
    }
}

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
    public record UpdateUserFieldsStateCommand(long UserId, FieldStateType State) : IRequest;

    public class UpdateUserFieldsStateHandler : IRequestHandler<UpdateUserFieldsStateCommand>
    {
        private readonly ILogger<UpdateUserFieldsStateHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateUserFieldsStateHandler(
            DbContextOptions options,
            ILogger<UpdateUserFieldsStateHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserFieldsStateCommand request, CancellationToken ct)
        {
            var entities = await _db.UserFields
                .AsQueryable()
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.State == FieldStateType.Planted)
                .ToListAsync();

            foreach (var field in entities)
            {
                field.State = request.State;
                field.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(field);

                _logger.LogInformation(
                    "Updated user {UserId} field {Number} to state {State}",
                    request.UserId, field.Number, request.State);
            }

            return Unit.Value;
        }
    }
}

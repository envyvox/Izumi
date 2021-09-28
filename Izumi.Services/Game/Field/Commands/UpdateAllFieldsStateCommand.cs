using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Field.Commands
{
    public record UpdateAllFieldsStateCommand(FieldStateType State) : IRequest;

    public class UpdateAllFieldsStateHandler : IRequestHandler<UpdateAllFieldsStateCommand>
    {
        private readonly ILogger<UpdateAllFieldsStateHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateAllFieldsStateHandler(
            DbContextOptions options,
            ILogger<UpdateAllFieldsStateHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAllFieldsStateCommand request, CancellationToken ct)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync($@"
                update user_fields
                set state = {request.State},
                    updated_at = {DateTimeOffset.UtcNow}
                where state != {FieldStateType.Empty}
                  and state != {FieldStateType.Completed}");

            _logger.LogInformation(
                "Fields state updated to {State}",
                request.State.ToString());

            return Unit.Value;
        }
    }
}

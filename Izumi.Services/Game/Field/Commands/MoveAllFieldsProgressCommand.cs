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
    public record MoveAllFieldsProgressCommand : IRequest;

    public class MoveAllFieldsProgressHandler : IRequestHandler<MoveAllFieldsProgressCommand>
    {
        private readonly ILogger<MoveAllFieldsProgressHandler> _logger;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public MoveAllFieldsProgressHandler(
            DbContextOptions options,
            ILogger<MoveAllFieldsProgressHandler> logger,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MoveAllFieldsProgressCommand request, CancellationToken ct)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync($@"
                update user_fields
                set progress = progress + 1,
                    updated_at = {DateTimeOffset.UtcNow}
                where state = {FieldStateType.Watered}");

            _logger.LogInformation(
                "Fields progress updated to +1");

            return await _mediator.Send(new CheckCompletedFieldsCommand());
        }
    }
}

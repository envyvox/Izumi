using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.User.Commands
{
    public record UpdateUserCommandColorCommand(long UserId, string Color) : IRequest;

    public class UpdateUserCommandColorHandler : IRequestHandler<UpdateUserCommandColorCommand>
    {
        private readonly ILogger<UpdateUserCommandColorHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateUserCommandColorHandler(
            DbContextOptions options,
            ILogger<UpdateUserCommandColorHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserCommandColorCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            entity.CommandColor = request.Color;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Updated user {UserId} command color to {Color}",
                request.UserId, request.Color);

            return Unit.Value;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.User.Commands
{
    public record UpdateUserLocationCommand(long UserId, LocationType Location) : IRequest;

    public class UpdateUserLocationHandler : IRequestHandler<UpdateUserLocationCommand>
    {
        private readonly ILogger<UpdateUserLocationHandler> _logger;
        private readonly AppDbContext _db;

        public UpdateUserLocationHandler(
            DbContextOptions options,
            ILogger<UpdateUserLocationHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateUserLocationCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            entity.Location = request.Location;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Updated user {UserId} location to {Location}",
                request.UserId, request.Location);

            return Unit.Value;
        }
    }
}

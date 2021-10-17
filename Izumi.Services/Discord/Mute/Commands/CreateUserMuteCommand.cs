using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Mute.Commands
{
    public record CreateUserMuteCommand(
            long UserId,
            long ModeratorId,
            uint Minutes,
            string Reason = null)
        : IRequest;

    public class CreateUserMuteHandler : IRequestHandler<CreateUserMuteCommand>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<CreateUserMuteHandler> _logger;

        public CreateUserMuteHandler(
            DbContextOptions options,
            ILogger<CreateUserMuteHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserMuteCommand request, CancellationToken cancellationToken)
        {
            await _db.CreateEntity(new UserMute
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ModeratorId = request.ModeratorId,
                Minutes = request.Minutes,
                Reason = request.Reason,
                CreatedAt = DateTimeOffset.UtcNow,
                Expiration = DateTimeOffset.UtcNow.AddMinutes(request.Minutes)
            });

            _logger.LogInformation(
                "Created user mute entity for user {UserId} with {Minutes} minutes and \"{Reason}\" reason from moderator {ModeratorId}",
                request.UserId, request.Minutes, request.Reason, request.ModeratorId);

            return Unit.Value;
        }
    }
}

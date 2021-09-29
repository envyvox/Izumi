using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Client.Events
{
    public record UserJoined(SocketGuildUser SocketGuildUser) : IRequest;

    public class UserJoinedHandler : IRequestHandler<UserJoined>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserJoinedHandler> _logger;

        public UserJoinedHandler(
            IMediator mediator,
            ILogger<UserJoinedHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(UserJoined request, CancellationToken ct)
        {
            _logger.LogInformation(
                "User {UserId} joined a guild",
                request.SocketGuildUser.Id);

            var user = await _mediator.Send(new GetUserQuery((long) request.SocketGuildUser.Id));
            var roles = await _mediator.Send(new GetRolesQuery());

            if (user.CreatedAt.Date != DateTimeOffset.UtcNow.Date &&
                request.SocketGuildUser.Roles.All(x => x.Name != user.Location.Role().Name()))
            {
                await request.SocketGuildUser.AddRoleAsync((ulong) roles[user.Location.Role()].Id);

                _logger.LogInformation(
                    "Added user {UserId} role {Role}",
                    request.SocketGuildUser.Id, user.Location.Role().ToString());
            }

            if (user.IsPremium && request.SocketGuildUser.Roles.All(x => x.Name != DiscordRoleType.Premium.Name()))
            {
                await request.SocketGuildUser.AddRoleAsync((ulong) roles[DiscordRoleType.Premium].Id);

                _logger.LogInformation(
                    "Added user {UserId} role {Role}",
                    request.SocketGuildUser.Id, DiscordRoleType.Premium.ToString());
            }

            return Unit.Value;
        }
    }
}

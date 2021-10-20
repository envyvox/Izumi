using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Discord.Mute.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using static Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Client.Events
{
    public record UserJoined(SocketGuildUser SocketGuildUser) : IRequest;

    public class UserJoinedHandler : IRequestHandler<UserJoined>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserJoinedHandler> _logger;
        private readonly Random _random = new();

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
            var muted = await _mediator.Send(new CheckUserMutedQuery(user.Id));

            if (user.CreatedAt.Date != DateTimeOffset.UtcNow.Date &&
                request.SocketGuildUser.Roles.All(x => x.Name != user.Location.Role().Name()))
            {
                await _mediator.Send(new AddRoleToGuildUserCommand(
                    request.SocketGuildUser.Id, user.Location.Role()));
            }

            if (user.IsPremium &&
                request.SocketGuildUser.Roles.All(x => x.Name != DiscordRoleType.Premium.Name()))
            {
                await _mediator.Send(new AddRoleToGuildUserCommand(
                    request.SocketGuildUser.Id, DiscordRoleType.Premium));
            }

            if (muted)
            {
                await _mediator.Send(new AddRoleToGuildUserCommand(
                    request.SocketGuildUser.Id, DiscordRoleType.Muted));
            }

            var channels = await _mediator.Send(new GetChannelsQuery());
            var channel = await _mediator.Send(new GetSocketTextChannelQuery(
                (ulong) channels[DiscordChannelType.Chat].Id));
            var randomWelcomeMessage = WelcomeMessages[_random.Next(0, WelcomeMessages.Length)];

            await channel.SendMessageAsync(string.Format(randomWelcomeMessage, request.SocketGuildUser.Mention));

            return Unit.Value;
        }
    }
}

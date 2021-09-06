using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record UserJoined(SocketGuildUser SocketGuildUser) : IRequest;

    public class UserJoinedHandler : IRequestHandler<UserJoined>
    {
        private readonly IMediator _mediator;

        public UserJoinedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UserJoined request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.SocketGuildUser.Id));
            var roles = await _mediator.Send(new GetRolesQuery());

            if (request.SocketGuildUser.Roles.All(x => x.Name != user.Location.Role().Name()))
            {
                await request.SocketGuildUser.AddRoleAsync((ulong) roles[user.Location.Role()].Id);
            }

            if (user.IsPremium && request.SocketGuildUser.Roles.All(x => x.Name != DiscordRoleType.Premium.Name()))
            {
                await request.SocketGuildUser.AddRoleAsync((ulong) roles[DiscordRoleType.Premium].Id);
            }

            return Unit.Value;
        }
    }
}

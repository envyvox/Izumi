using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
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

            // todo send welcome message maybe?

            return Unit.Value;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace Izumi.Services.Discord.Guild.Queries
{
    public record GetSocketGuildUserQuery(ulong UserId) : IRequest<SocketGuildUser>;

    public class GetSocketGuildUserHandler : IRequestHandler<GetSocketGuildUserQuery, SocketGuildUser>
    {
        private readonly IMediator _mediator;

        public GetSocketGuildUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<SocketGuildUser> Handle(GetSocketGuildUserQuery request, CancellationToken ct)
        {
            var socketGuild = await _mediator.Send(new GetSocketGuildQuery());

            await socketGuild.DownloadUsersAsync();

            var socketUser = socketGuild.GetUser(request.UserId);

            if (socketUser is null)
            {
                throw new Exception($"socket user {request.UserId} was not found in guild {socketGuild.Id}");
            }

            return socketUser;
        }
    }
}

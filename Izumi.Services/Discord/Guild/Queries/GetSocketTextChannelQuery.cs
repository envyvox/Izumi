using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace Izumi.Services.Discord.Guild.Queries
{
    public record GetSocketTextChannelQuery(ulong Id) : IRequest<SocketTextChannel>;

    public class GetSocketTextChannelHandler : IRequestHandler<GetSocketTextChannelQuery, SocketTextChannel>
    {
        private readonly IMediator _mediator;

        public GetSocketTextChannelHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<SocketTextChannel> Handle(GetSocketTextChannelQuery request, CancellationToken ct)
        {
            var guild = await _mediator.Send(new GetSocketGuildQuery());

            try
            {
                return guild.GetTextChannel(request.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

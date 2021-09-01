using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;

namespace Izumi.Services.Discord.Guild.Queries
{
    public record GetUserMessageQuery(ulong ChannelId, ulong MessageId) : IRequest<IUserMessage>;

    public class GetUserMessageHandler : IRequestHandler<GetUserMessageQuery, IUserMessage>
    {
        private readonly IMediator _mediator;

        public GetUserMessageHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IUserMessage> Handle(GetUserMessageQuery request, CancellationToken ct)
        {
            var channel = await _mediator.Send(new GetSocketTextChannelQuery(request.ChannelId));

            try
            {
                return (IUserMessage) await channel.GetMessageAsync(request.MessageId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Services.Discord.CommunityDesc.Commands;
using Izumi.Services.Discord.Guild.Extensions;
using Izumi.Services.Extensions;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record MessageDeleted(
            Cacheable<IMessage, ulong> Message,
            Cacheable<IMessageChannel, ulong> Channel)
        : IRequest;

    public class MessageDeletedHandler : IRequestHandler<MessageDeleted>
    {
        private readonly IMediator _mediator;

        public MessageDeletedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MessageDeleted request, CancellationToken cancellationToken)
        {
            var msg = await request.Message.GetOrDownloadAsync();
            var channels = DiscordRepository.Channels;
            var communityDescChannels = channels.GetCommunityDescChannels();

            if (communityDescChannels.Contains(request.Channel.Id))
            {
                await _mediator.Send(new DeleteContentMessageCommand((long) request.Channel.Id, (long) msg.Id));
            }

            return Unit.Value;
        }
    }
}

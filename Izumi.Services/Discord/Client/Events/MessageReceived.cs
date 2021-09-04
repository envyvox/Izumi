using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.CommunityDesc.Commands;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Extensions;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.Statistic.Commands;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record MessageReceived(SocketMessage SocketMessage) : IRequest;

    public class MessageReceivedHandler : IRequestHandler<MessageReceived>
    {
        private readonly IMediator _mediator;

        public MessageReceivedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MessageReceived request, CancellationToken cancellationToken)
        {
            var channels = await _mediator.Send(new GetChannelsQuery());
            var communityDescChannels = channels.GetCommunityDescChannels();

            if (communityDescChannels.Contains(request.SocketMessage.Channel.Id))
            {
                var hasAttachment =
                    request.SocketMessage.Attachments.Count == 1 ||
                    request.SocketMessage.Content.Contains("http");

                if (hasAttachment)
                {
                    await AddVotes((IUserMessage) request.SocketMessage);
                    await _mediator.Send(new CreateContentMessageCommand(
                        (long) request.SocketMessage.Author.Id, (long) request.SocketMessage.Channel.Id,
                        (long) request.SocketMessage.Id));
                }
                else
                {
                    await Task.Delay(1000);
                    await request.SocketMessage.DeleteAsync();
                }
            }

            if (request.SocketMessage.Channel.Id == (ulong) channels[DiscordChannelType.Suggestions].Id)
            {
                await AddVotes((IUserMessage) request.SocketMessage);
            }

            if (request.SocketMessage.Channel.Id == (ulong) channels[DiscordChannelType.Chat].Id)
            {
                await _mediator.Send(new AddStatisticToUserCommand(
                    (long) request.SocketMessage.Author.Id, StatisticType.Messages));
                // TODO check achievement "first message"
            }

            return Unit.Value;
        }

        private async Task AddVotes(IUserMessage message)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());

            await message.AddReactionsAsync(new IEmote[]
            {
                global::Discord.Emote.Parse(emotes.GetEmote("Like")),
                global::Discord.Emote.Parse(emotes.GetEmote("Dislike"))
            });
        }
    }
}

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.CommunityDesc.Commands;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Guild.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.User.Queries;
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
            var channels = DiscordRepository.Channels;
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
                    await DeleteMessage(request.SocketMessage);
                }
            }

            if (request.SocketMessage.Channel.Id == channels[DiscordChannelType.Suggestions].Id)
            {
                await AddVotes((IUserMessage) request.SocketMessage);
            }

            if (request.SocketMessage.Channel.Id == channels[DiscordChannelType.Chat].Id)
            {
                var user = await _mediator.Send(new GetUserQuery((long) request.SocketMessage.Author.Id));

                await _mediator.Send(new AddStatisticToUserCommand(user.Id, StatisticType.Messages));
                await _mediator.Send(new CheckAchievementInUserCommand(user.Id, AchievementType.FirstMessage));
            }

            if (request.SocketMessage.Channel.Id == channels[DiscordChannelType.Commands].Id)
            {
                await DeleteMessage(request.SocketMessage);
            }

            return Unit.Value;
        }

        private async Task AddVotes(IUserMessage message)
        {
            var emotes = DiscordRepository.Emotes;

            await message.AddReactionsAsync(new IEmote[]
            {
                global::Discord.Emote.Parse(emotes.GetEmote("Like")),
                global::Discord.Emote.Parse(emotes.GetEmote("Dislike"))
            });
        }

        private async Task DeleteMessage(SocketMessage message)
        {
            // delay cuz discord cache
            await Task.Delay(1000);
            await message.DeleteAsync();
        }
    }
}

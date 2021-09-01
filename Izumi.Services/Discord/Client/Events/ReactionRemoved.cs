using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.CommunityDesc.Commands;
using Izumi.Services.Discord.CommunityDesc.Queries;
using Izumi.Services.Discord.Guild.Extensions;
using Izumi.Services.Discord.Guild.Queries;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record ReactionRemoved(
            Cacheable<IUserMessage, ulong> Message,
            Cacheable<IMessageChannel, ulong> Channel,
            SocketReaction Reaction)
        : IRequest;

    public class ReactionRemovedHandler : IRequestHandler<ReactionRemoved>
    {
        private readonly IMediator _mediator;

        public ReactionRemovedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ReactionRemoved request, CancellationToken cancellationToken)
        {
            if (request.Reaction.User.Value.IsBot) return Unit.Value;

            var channels = await _mediator.Send(new GetChannelsQuery());
            var communityDescChannels = channels.GetCommunityDescChannels();

            if (communityDescChannels.Contains(request.Channel.Id))
            {
                if (request.Reaction.Emote.Name is not ("Like" or "Dislike")) return Unit.Value;

                var contentMessage = await _mediator.Send(new GetContentMessageByParamsQuery(
                    (long) request.Channel.Id, (long) request.Message.Id));

                await _mediator.Send(new DeactivateUserVoteCommand(
                    (long) request.Reaction.UserId, contentMessage.Id,
                    request.Reaction.Emote.Name == "Like" ? VoteType.Like : VoteType.Dislike));
            }

            return Unit.Value;
        }
    }
}

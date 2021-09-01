using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.CommunityDesc.Commands;
using Izumi.Services.Discord.CommunityDesc.Queries;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Extensions;
using Izumi.Services.Discord.Guild.Queries;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record ReactionAdded(
            Cacheable<IUserMessage, ulong> Message,
            Cacheable<IMessageChannel, ulong> Channel,
            SocketReaction Reaction)
        : IRequest;

    public class ReactionAddedHandler : IRequestHandler<ReactionAdded>
    {
        private readonly IMediator _mediator;

        public ReactionAddedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ReactionAdded request, CancellationToken cancellationToken)
        {
            if (request.Reaction.User.Value.IsBot) return Unit.Value;

            var msg = await request.Message.GetOrDownloadAsync();
            var channels = await _mediator.Send(new GetChannelsQuery());
            var communityDescChannels = channels.GetCommunityDescChannels();

            if (request.Channel.Id == (ulong) channels[DiscordChannelType.SearchGetRoles].Id ||
                request.Channel.Id == (ulong) channels[DiscordChannelType.EventGetRoles].Id)
            {
                var role = request.Reaction.Emote.Name switch
                {
                    // роль мероприятий
                    "🥳" => DiscordRoleType.DiscordEvent,
                    // роли оповещений событий
                    "NumOne" => DiscordRoleType.AllEvents,
                    "NumTwo" => DiscordRoleType.DailyEvents,
                    "NumThree" => DiscordRoleType.WeeklyEvents,
                    "NumFour" => DiscordRoleType.MonthlyEvents,
                    "NumFive" => DiscordRoleType.YearlyEvents,
                    "NumSix" => DiscordRoleType.UniqueEvents,
                    // игровые роли
                    "GenshinImpact" => DiscordRoleType.GenshinImpact,
                    "LeagueOfLegends" => DiscordRoleType.LeagueOfLegends,
                    "TeamfightTactics" => DiscordRoleType.TeamfightTactics,
                    "Valorant" => DiscordRoleType.Valorant,
                    "ApexLegends" => DiscordRoleType.ApexLegends,
                    "LostArk" => DiscordRoleType.LostArk,
                    "Dota" => DiscordRoleType.Dota,
                    "Osu" => DiscordRoleType.Osu,
                    "AmongUs" => DiscordRoleType.AmongUs,
                    "Rust" => DiscordRoleType.Rust,
                    "CSGO" => DiscordRoleType.CsGo,
                    "HotS" => DiscordRoleType.HotS,
                    "WildRift" => DiscordRoleType.WildRift,
                    "MobileLegends" => DiscordRoleType.MobileLegends,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var hasRole = await _mediator.Send(new CheckGuildUserHasRoleQuery(request.Reaction.UserId, role));

                if (hasRole)
                {
                    await _mediator.Send(new RemoveRoleFromGuildUserCommand(request.Reaction.UserId, role));

                }
                else
                {
                    await _mediator.Send(new AddRoleToGuildUserCommand(request.Reaction.UserId, role));
                }

                await msg.RemoveReactionAsync(request.Reaction.Emote, request.Reaction.UserId);
            }

            if (communityDescChannels.Contains(request.Channel.Id))
            {
                if (request.Reaction.Emote.Name is not ("Like" or "Dislike")) return Unit.Value;

                var contentMessage = await _mediator.Send(new GetContentMessageByParamsQuery(
                    (long) request.Channel.Id, (long) request.Message.Id));

                if (request.Reaction.UserId == (ulong) contentMessage.User.Id)
                {
                    await msg.RemoveReactionAsync(request.Reaction.Emote, request.Reaction.UserId);
                }
                else
                {
                    var vote = request.Reaction.Emote.Name == "Like" ? VoteType.Like : VoteType.Dislike;
                    var antiVote = vote == VoteType.Like ? VoteType.Dislike : VoteType.Like;
                    var userVotes = await _mediator.Send(new GetUserVotesOnMessageQuery(
                        (long) request.Reaction.UserId, contentMessage.Id));

                    if (userVotes.ContainsKey(antiVote) && userVotes[antiVote].IsActive)
                    {
                        var emotes = await _mediator.Send(new GetEmotesQuery());

                        await msg.RemoveReactionAsync(
                            antiVote == VoteType.Like
                                ? global::Discord.Emote.Parse(emotes.GetEmote("Like"))
                                : global::Discord.Emote.Parse(emotes.GetEmote("Dislike")),
                            request.Reaction.UserId);
                    }

                    if (userVotes.ContainsKey(vote))
                    {
                        await _mediator.Send(new ActivateUserVoteCommand(
                            (long) request.Reaction.UserId, contentMessage.Id, vote));
                    }
                    else
                    {
                        await _mediator.Send(new CreateUserVoteCommand(
                            (long) request.Reaction.UserId, contentMessage.Id, vote));
                    }
                }
            }

            return Unit.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.CommunityDesc.Models;
using Izumi.Services.Discord.CommunityDesc.Queries;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Models;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Discord.Role.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info
{
    public record CommunityDescCommand(SocketSlashCommand Command) : IRequest;

    public class CommunityDescHandler : IRequestHandler<CommunityDescCommand>
    {
        private readonly IMediator _mediator;
        private Dictionary<string, EmoteDto> _emotes;

        public CommunityDescHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CommunityDescCommand request, CancellationToken ct)
        {
            _emotes = await _mediator.Send(new GetEmotesQuery());
            var channels = await _mediator.Send(new GetChannelsQuery());
            var roles = await _mediator.Send(new GetRolesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userMessages = await _mediator.Send(new GetContentMessagesByUserIdQuery(user.Id));
            var userVotes = await _mediator.Send(new GetContentAuthorVotesQuery(user.Id));
            var hasRole = await _mediator.Send(new CheckGuildUserHasRoleQuery(
                request.Command.User.Id, DiscordRoleType.ContentProvider));

            var photosMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannelType.Photos].Id)
                .ToList();
            var screenshotMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannelType.Screenshots].Id)
                .ToList();
            var memesMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannelType.Memes].Id)
                .ToList();
            var artMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannelType.Arts].Id)
                .ToList();
            var eroticMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannelType.Erotic].Id)
                .ToList();
            var nsfwMessages = userMessages
                .Where(x => x.ChannelId == channels[DiscordChannelType.Nsfw].Id)
                .ToList();

            var photosMessagesLikes = ChannelMessagesVotes(userVotes, photosMessages, VoteType.Like);
            var photosMessagesDislikes = ChannelMessagesVotes(userVotes, photosMessages, VoteType.Dislike);
            var screenshotMessagesLikes = ChannelMessagesVotes(userVotes, screenshotMessages, VoteType.Like);
            var screenshotMessagesDislikes = ChannelMessagesVotes(userVotes, screenshotMessages, VoteType.Dislike);
            var memesMessagesLikes = ChannelMessagesVotes(userVotes, memesMessages, VoteType.Like);
            var memesMessagesDislikes = ChannelMessagesVotes(userVotes, memesMessages, VoteType.Dislike);
            var artMessagesLikes = ChannelMessagesVotes(userVotes, artMessages, VoteType.Like);
            var artMessagesDislikes = ChannelMessagesVotes(userVotes, artMessages, VoteType.Dislike);
            var eroticMessagesLikes = ChannelMessagesVotes(userVotes, eroticMessages, VoteType.Like);
            var eroticMessagesDislikes = ChannelMessagesVotes(userVotes, eroticMessages, VoteType.Dislike);
            var nsfwMessagesLikes = ChannelMessagesVotes(userVotes, nsfwMessages, VoteType.Like);
            var nsfwMessagesDislikes = ChannelMessagesVotes(userVotes, nsfwMessages, VoteType.Dislike);

            var embed = new EmbedBuilder()
                .WithAuthor("Доска сообщества")
                .WithDescription(
                    $"{_emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут собрана информация о твоем участии в доске сообщества: " +
                    $"\n{_emotes.GetEmote("Blank")}")
                .AddField("Публикации",
                    DisplayChannelInfo(photosMessages.Count, channels[DiscordChannelType.Photos].Id,
                        photosMessagesLikes, photosMessagesDislikes) +
                    DisplayChannelInfo(screenshotMessages.Count, channels[DiscordChannelType.Screenshots].Id,
                        screenshotMessagesLikes, screenshotMessagesDislikes) +
                    DisplayChannelInfo(memesMessages.Count, channels[DiscordChannelType.Memes].Id,
                        memesMessagesLikes, memesMessagesDislikes) +
                    DisplayChannelInfo(artMessages.Count, channels[DiscordChannelType.Arts].Id,
                        artMessagesLikes, artMessagesDislikes) +
                    DisplayChannelInfo(eroticMessages.Count, channels[DiscordChannelType.Erotic].Id,
                        eroticMessagesLikes, eroticMessagesDislikes) +
                    DisplayChannelInfo(nsfwMessages.Count, channels[DiscordChannelType.Nsfw].Id,
                        nsfwMessagesLikes, nsfwMessagesDislikes) +
                    $"Всего {_emotes.GetEmote(VoteType.Like.ToString())} {userVotes.Count(x => x.Vote == VoteType.Like)} лайков");

            if (hasRole)
            {
                var userRole = await _mediator.Send(new GetUserRoleQuery(
                    user.Id, roles[DiscordRoleType.ContentProvider].Id));
                var roleEndString = (DateTimeOffset.UtcNow - userRole.Expiration).TotalDays
                    .Days()
                    .Humanize(2, new CultureInfo("ru-RU"));

                embed.AddField("Поставщик контента",
                    $"Роль будет снята через {roleEndString}");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }

        private static int ChannelMessagesVotes(List<ContentVoteDto> votes, List<ContentMessageDto> messages,
            VoteType vote)
        {
            return votes
                .Where(cv => messages
                    .Any(cm => cv.Message.Id == cm.Id))
                .Count(cv => cv.Vote == vote);
        }

        private string DisplayChannelInfo(long messages, long channelId, long likes, long dislikes)
        {
            return
                $"{_emotes.GetEmote("List")} {messages} публикаций в <#{channelId}>, " +
                $"{_emotes.GetEmote(VoteType.Like.ToString())} {likes} лайков " +
                $"и {_emotes.GetEmote(VoteType.Dislike.ToString())} {dislikes} дизлайков\n";
        }
    }
}

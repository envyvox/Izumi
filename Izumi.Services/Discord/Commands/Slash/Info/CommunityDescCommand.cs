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
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Discord.CommunityDesc.Models;
using Izumi.Services.Discord.CommunityDesc.Queries;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Models;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Discord.Role.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Options;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Info
{
    public record CommunityDescCommand(SocketSlashCommand Command) : IRequest;

    public class CommunityDescHandler : IRequestHandler<CommunityDescCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly DiscordOptions _options;
        private Dictionary<string, EmoteDto> _emotes;

        public CommunityDescHandler(
            IMediator mediator,
            ILocalizationService local,
            IOptions<DiscordOptions> options)
        {
            _mediator = mediator;
            _local = local;
            _options = options.Value;
        }

        public async Task<Unit> Handle(CommunityDescCommand request, CancellationToken ct)
        {
            _emotes = DiscordRepository.Emotes;
            var channels = DiscordRepository.Channels;
            var roles = DiscordRepository.Roles;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userMessages = await _mediator.Send(new GetContentMessagesByUserIdQuery(user.Id));
            var userVotes = await _mediator.Send(new GetContentAuthorVotesQuery(user.Id));
            var hasRole = await _mediator.Send(new CheckGuildUserHasRoleQuery(
                request.Command.User.Id, DiscordRoleType.ContentProvider));

            var photosMessages = userMessages
                .Where(x => x.ChannelId == (long) channels[DiscordChannelType.Photos].Id)
                .ToList();
            var screenshotMessages = userMessages
                .Where(x => x.ChannelId == (long) channels[DiscordChannelType.Screenshots].Id)
                .ToList();
            var memesMessages = userMessages
                .Where(x => x.ChannelId == (long) channels[DiscordChannelType.Memes].Id)
                .ToList();
            var artMessages = userMessages
                .Where(x => x.ChannelId == (long) channels[DiscordChannelType.Arts].Id)
                .ToList();
            var musicMessages = userMessages
                .Where(x => x.ChannelId == (long) channels[DiscordChannelType.Music].Id)
                .ToList();
            var eroticMessages = userMessages
                .Where(x => x.ChannelId == (long) channels[DiscordChannelType.Erotic].Id)
                .ToList();
            var nsfwMessages = userMessages
                .Where(x => x.ChannelId == (long) channels[DiscordChannelType.Nsfw].Id)
                .ToList();

            var photosMessagesLikes = (uint) ChannelMessagesVotes(userVotes, photosMessages, VoteType.Like);
            var photosMessagesDislikes = (uint) ChannelMessagesVotes(userVotes, photosMessages, VoteType.Dislike);
            var screenshotMessagesLikes = (uint) ChannelMessagesVotes(userVotes, screenshotMessages, VoteType.Like);
            var screenshotMessagesDislikes =
                (uint) ChannelMessagesVotes(userVotes, screenshotMessages, VoteType.Dislike);
            var memesMessagesLikes = (uint) ChannelMessagesVotes(userVotes, memesMessages, VoteType.Like);
            var memesMessagesDislikes = (uint) ChannelMessagesVotes(userVotes, memesMessages, VoteType.Dislike);
            var artMessagesLikes = (uint) ChannelMessagesVotes(userVotes, artMessages, VoteType.Like);
            var artMessagesDislikes = (uint) ChannelMessagesVotes(userVotes, artMessages, VoteType.Dislike);
            var musicMessagesLikes = (uint) ChannelMessagesVotes(userVotes, musicMessages, VoteType.Like);
            var musicMessagesDislikes = (uint) ChannelMessagesVotes(userVotes, musicMessages, VoteType.Dislike);
            var eroticMessagesLikes = (uint) ChannelMessagesVotes(userVotes, eroticMessages, VoteType.Like);
            var eroticMessagesDislikes = (uint) ChannelMessagesVotes(userVotes, eroticMessages, VoteType.Dislike);
            var nsfwMessagesLikes = (uint) ChannelMessagesVotes(userVotes, nsfwMessages, VoteType.Like);
            var nsfwMessagesDislikes = (uint) ChannelMessagesVotes(userVotes, nsfwMessages, VoteType.Dislike);

            var totalLikes = (uint) userVotes.Count(x => x.Vote == VoteType.Like);
            var embed = new EmbedBuilder()
                .WithAuthor("Доска сообщества")
                .WithDescription(
                    $"{_emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут собрана информация о твоем участии в доске сообщества: " +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField(DiscordChannelType.Photos.Name(),
                    DisplayChannelInfo((uint) photosMessages.Count,
                        photosMessagesLikes, photosMessagesDislikes), true)
                .AddField(DiscordChannelType.Screenshots.Name(),
                    DisplayChannelInfo((uint) screenshotMessages.Count,
                        screenshotMessagesLikes, screenshotMessagesDislikes), true)
                .AddField(DiscordChannelType.Memes.Name(),
                    DisplayChannelInfo((uint) memesMessages.Count,
                        memesMessagesLikes, memesMessagesDislikes), true)
                .AddField(DiscordChannelType.Arts.Name(),
                    DisplayChannelInfo((uint) artMessages.Count,
                        artMessagesLikes, artMessagesDislikes), true)
                .AddField(DiscordChannelType.Music.Name(),
                    DisplayChannelInfo((uint) musicMessages.Count,
                        musicMessagesLikes, musicMessagesDislikes), true)
                .AddField(DiscordChannelType.Erotic.Name(),
                    DisplayChannelInfo((uint) eroticMessages.Count,
                        eroticMessagesLikes, eroticMessagesDislikes), true)
                .AddField(DiscordChannelType.Nsfw.Name(),
                    DisplayChannelInfo((uint) nsfwMessages.Count,
                        nsfwMessagesLikes, nsfwMessagesDislikes), true)
                // .AddEmptyField(true)
                .AddField("Всего",
                    $"{_emotes.GetEmote(VoteType.Like.ToString())} {totalLikes} " +
                    $"{_local.Localize(LocalizationCategoryType.Vote, VoteType.Like.ToString(), totalLikes)}");

            if (hasRole)
            {
                var userRole = await _mediator.Send(new GetUserRoleQuery(
                    user.Id, (long) roles[DiscordRoleType.ContentProvider].Id));
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

        private string DisplayChannelInfo(uint messages, uint likes, uint dislikes)
        {
            return
                $"{_emotes.GetEmote("List")} {messages} {_local.Localize(LocalizationCategoryType.Basic, "Post", messages)}" +
                $"\n{_emotes.GetEmote(VoteType.Like.ToString())} {likes} {_local.Localize(LocalizationCategoryType.Vote, VoteType.Like.ToString(), likes)}" +
                $"\n{_emotes.GetEmote(VoteType.Dislike.ToString())} {dislikes} {_local.Localize(LocalizationCategoryType.Vote, VoteType.Dislike.ToString(), dislikes)}";
        }
    }
}
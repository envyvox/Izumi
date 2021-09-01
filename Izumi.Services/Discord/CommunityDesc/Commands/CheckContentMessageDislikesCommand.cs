using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.CommunityDesc.Queries;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Queries;
using MediatR;

namespace Izumi.Services.Discord.CommunityDesc.Commands
{
    public record CheckContentMessageDislikesCommand(Guid ContentMessageId) : IRequest;

    public class CheckContentMessageDislikesHandler : IRequestHandler<CheckContentMessageDislikesCommand>
    {
        private readonly IMediator _mediator;

        public CheckContentMessageDislikesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckContentMessageDislikesCommand request, CancellationToken cancellationToken)
        {
            var messageDislikes = await _mediator.Send(new GetContentMessageVotesQuery(
                request.ContentMessageId, VoteType.Dislike));

            if (messageDislikes.Count >= 5)
            {
                var message = await _mediator.Send(new GetUserMessageQuery(
                    (ulong) messageDislikes[0].Message.ChannelId, (ulong) messageDislikes[0].Message.MessageId));
                var emotes = await _mediator.Send(new GetEmotesQuery());

                var embed = new EmbedBuilder()
                    .WithAuthor("Оповещение от доски сообщества")
                    .WithDescription(
                        $"Твоя публикация собрала {emotes.GetEmote("Dislike")} 5 дизлайков и была автоматически удалена из <#{message.Channel.Id}>.")
                    .WithImageUrl(message.Attachments.First().Url);

                await _mediator.Send(new SendEmbedToUserCommand(message.Author.Id, embed));
                await message.DeleteAsync();
            }

            return Unit.Value;
        }
    }
}

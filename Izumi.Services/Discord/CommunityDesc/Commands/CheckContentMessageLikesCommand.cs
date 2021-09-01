using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.CommunityDesc.Queries;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Discord.Role.Commands;
using MediatR;

namespace Izumi.Services.Discord.CommunityDesc.Commands
{
    public record CheckContentMessageLikesCommand(Guid ContentMessageId) : IRequest;

    public class CheckContentMessageLikesHandler : IRequestHandler<CheckContentMessageLikesCommand>
    {
        private readonly IMediator _mediator;

        public CheckContentMessageLikesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckContentMessageLikesCommand request, CancellationToken cancellationToken)
        {
            var contentMessage = await _mediator.Send(new GetContentMessageQuery(request.ContentMessageId));
            var authorLikes = await _mediator.Send(
                new GetContentAuthorVotesCountQuery(contentMessage.User.Id, VoteType.Like));

            if (authorLikes % 500 == 0)
            {
                var emotes = await _mediator.Send(new GetEmotesQuery());
                var roles = await _mediator.Send(new GetRolesQuery());

                await _mediator.Send(new AddRoleToGuildUserCommand(
                    (ulong) contentMessage.User.Id, DiscordRoleType.ContentProvider));
                await _mediator.Send(new AddRoleToUserCommand(
                    contentMessage.User.Id, roles[DiscordRoleType.ContentProvider].Id, TimeSpan.FromDays(30)));

                var embed = new EmbedBuilder()
                    .WithAuthor("Оповещение от доски сообщества")
                    .WithDescription(
                        $"Твои публикации в **доске сообщества** были {emotes.GetEmote("Like")} оценены, " +
                        $"за что ты получаешь роль **{DiscordRoleType.ContentProvider.Name()}** на 30 дней.");

                await _mediator.Send(new SendEmbedToUserCommand((ulong) contentMessage.User.Id, embed));
            }

            return Unit.Value;
        }
    }
}

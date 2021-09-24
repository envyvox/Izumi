using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.Referral.Commands;
using Izumi.Services.Game.Referral.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Referral
{
    public record ReferralSetCommand(SocketSlashCommand Command) : IRequest;

    public class ReferralSetHandler : IRequestHandler<ReferralSetCommand>
    {
        private readonly IMediator _mediator;

        public ReferralSetHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ReferralSetCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var tSocketUser = (IGuildUser) request.Command.Data.Options.First().Value;

            var embed = new EmbedBuilder()
                .WithAuthor("Реферальная система");

            if (user.Id == (long) tSocketUser.Id)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты не можешь указать самого себя как пригласившего тебя пользователя.");
            }
            else if (tSocketUser.IsBot)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты не можешь указать бота как пригласившего тебя пользователя.");
            }
            else
            {
                var hasReferrer = await _mediator.Send(new CheckUserHasReferrerQuery(user.Id));

                if (hasReferrer)
                {
                    var rUser = await _mediator.Send(new GetUserReferrerQuery(user.Id));
                    var rSocketUser = await _mediator.Send(new GetSocketGuildUserQuery((ulong) rUser.Id));

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты уже указал {emotes.GetEmote(rUser.Title.EmoteName())} {rUser.Title.Localize()} {rSocketUser.Mention} " +
                        "как пригласившего тебя пользователя.");
                }
                else
                {
                    var tUser = await _mediator.Send(new GetUserQuery((long) tSocketUser.Id));

                    await _mediator.Send(new CreateUserReferrerCommand(user.Id, tUser.Id));

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно указал {emotes.GetEmote(tUser.Title.EmoteName())} {tUser.Title.Localize()} {tSocketUser.Mention} " +
                        "как пригласившего тебя пользователя.");
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

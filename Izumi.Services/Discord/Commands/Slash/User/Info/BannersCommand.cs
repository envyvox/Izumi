using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Banner.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info
{
    public record BannersCommand(SocketSlashCommand Command) : IRequest;

    public class BannersHandler : IRequestHandler<BannersCommand>
    {
        private readonly IMediator _mediator;

        public BannersHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(BannersCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userBanners = await _mediator.Send(new GetUserBannersQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Коллекция баннеров")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются твои баннеры:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/баннер [номер баннера]` чтобы изменить текущий баннер." +
                    $"\n{emotes.GetEmote("Blank")}");

            var counter = 0;
            foreach (var userBanner in userBanners.Take(16))
            {
                counter++;

                embed.AddField(
                    $"{emotes.GetEmote("List")} `{userBanner.Banner.AutoIncrementedId}` «{userBanner.Banner.Name}»",
                    $"[Нажми сюда чтобы посмотреть]({userBanner.Banner.Url})", true);

                if (counter == 2)
                {
                    counter = 0;

                    embed.AddField(StringExtensions.EmptyChar, StringExtensions.EmptyChar, true);
                }

                if (userBanners.Count > 16) embed.WithFooter("Тут отображаются только первые 16 твоих баннеров.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

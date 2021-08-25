using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.Banner.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.SlashCommands.Commands.User.Info
{
    public record ProfileCommand(SocketSlashCommand Command) : IRequest;

    public class ProfileHandler : IRequestHandler<ProfileCommand>
    {
        private readonly IMediator _mediator;

        public ProfileHandler(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProfileCommand request, CancellationToken ct)
        {
            var socketUser = (SocketGuildUser) request.Command.Data.Options.First().Value ?? request.Command.User;
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) socketUser.Id));
            // var banner = await _mediator.Send(new GetUserActiveBannerQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Профиль")
                .WithThumbnailUrl(socketUser.GetAvatarUrl())
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {socketUser.Mention}")
                .AddField("Пол",
                    $"{emotes.GetEmote(user.Gender.EmoteName())} {user.Gender.Localize()}" +
                    $"\n{StringExtensions.EmptyChar}", true)
                .AddField("День рождения",
                    $"{emotes.GetEmote("Blank")} WIP...", true)
                .AddField("Энергия",
                    $"{emotes.GetEmote("Energy")} {user.Energy}")
                .AddField("Текущая локация",
                    $"{user.Location.Localize()}" +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Рейтинг приключений",
                    $"{emotes.GetEmote("Blank")} WIP...")
                .AddField("Семья",
                    $"{emotes.GetEmote("Blank")} WIP..." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Дата присоединения",
                    user.CreatedAt.ToString("dd MMMM yyy", new CultureInfo("ru-RU")))
                .AddField("Информация",
                    user.About ?? "Тут пока что ничего не указано, но я уверена что это отличный пользователь.");

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

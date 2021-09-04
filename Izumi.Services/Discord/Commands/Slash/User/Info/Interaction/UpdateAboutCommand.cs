using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.Cooldown.Commands;
using Izumi.Services.Game.Cooldown.Queries;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info.Interaction
{
    public record UpdateAboutCommand(SocketSlashCommand Command) : IRequest;

    public class UpdateAboutHandler : IRequestHandler<UpdateAboutCommand>
    {
        private readonly IMediator _mediator;

        public UpdateAboutHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateAboutCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userCooldown = await _mediator.Send(new GetUserCooldownQuery(user.Id, CooldownType.UpdateAbout));

            if (userCooldown.Expiration > DateTimeOffset.UtcNow)
            {
                await Task.FromException(new Exception(
                    "обновление информации в профиле доступно через " +
                    $"{(userCooldown.Expiration - DateTimeOffset.UtcNow).TotalHours.Hours().Humanize(2, new CultureInfo("ru-RU"))}."));
            }
            else
            {
                var newInfo = (string) request.Command.Data.Options.First().Value;

                if (newInfo.Length is < 2 or > 1024)
                {
                    await Task.FromException(new Exception(
                        "информация профиля должна быть длинее 1 и короче 1024 символов"));
                }
                else
                {
                    var emotes = await _mediator.Send(new GetEmotesQuery());

                    await _mediator.Send(new UpdateUserAboutCommand(user.Id, newInfo));
                    await _mediator.Send(new CreateUserCooldownCommand(
                        user.Id, CooldownType.UpdateAbout, TimeSpan.FromDays(3)));

                    var embed = new EmbedBuilder()
                        .WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            "информация в твоем профиле успешно обновлена.");

                    return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
                }
            }

            return Unit.Value;
        }
    }
}

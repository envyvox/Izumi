using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Settings
{
    public record SettingsAutoTitleRoleCommand(SocketSlashCommand Command) : IRequest;

    public class SettingsAutoTitleRoleCommandHandler : IRequestHandler<SettingsAutoTitleRoleCommand>
    {
        private readonly IMediator _mediator;

        public SettingsAutoTitleRoleCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingsAutoTitleRoleCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var guildUser = await _mediator.Send(new GetSocketGuildUserQuery(request.Command.User.Id));
            var roles = DiscordRepository.Roles;

            var embed = new EmbedBuilder();

            if (user.AutoTitleRole)
            {
                await guildUser.RemoveRoleAsync(roles[user.Title.Role()].Id);
                await _mediator.Send(new UpdateUserAutoTitleRoleCommand(user.Id, false));

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты отключил автоматическую выдачу роли активного титула. " +
                    $"Роль <@&{roles[user.Title.Role()].Id}> была снята.");
            }
            else
            {
                await guildUser.AddRoleAsync(roles[user.Title.Role()].Id);
                await _mediator.Send(new UpdateUserAutoTitleRoleCommand(user.Id, true));

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты включил автоматическую выдачу роли активного титула. " +
                    $"Роль <@&{roles[user.Title.Role()].Id}> была добавлена.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

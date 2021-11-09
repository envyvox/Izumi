using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Commands.Prefix.Attributes;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Hangfire.Commands;
using MediatR;

namespace Izumi.Services.Discord.Commands.Prefix
{
    [RequireRole(DiscordRoleType.Moderator)]
    public class Unmute : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public Unmute(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("unmute")]
        public async Task UnmuteTask(IUser mentionedUser)
        {
            var guildUser = await _mediator.Send(new GetSocketGuildUserQuery(mentionedUser.Id));
            var hasRole = await _mediator.Send(new CheckGuildUserHasRoleQuery(guildUser.Id, DiscordRoleType.Muted));

            var embed = new EmbedBuilder();

            if (hasRole is false)
            {
                embed.WithDescription($"Пользователь {guildUser.Mention} не находится в блокировке чата.");
            }
            else
            {
                await _mediator.Send(new RemoveRoleFromGuildUserCommand(guildUser.Id, DiscordRoleType.Muted));
                await _mediator.Send(new DeleteUserHangfireJobCommand((long) guildUser.Id, HangfireJobType.Unmute));

                embed.WithDescription($"С пользователя {guildUser.Mention} снята блокировка чата.");

                var notify = new EmbedBuilder()
                    .WithDescription("С тебя преждевременно снята блокировка чата.");

                await _mediator.Send(new SendEmbedToUserCommand(guildUser.Id, notify));
            }

            await _mediator.Send(new SendEmbedToUserCommand(Context.User.Id, embed));
        }
    }
}

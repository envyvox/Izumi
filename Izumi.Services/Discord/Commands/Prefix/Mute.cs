using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Commands.Prefix.Attributes;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Discord.Mute.Commands;
using Izumi.Services.Discord.Mute.Queries;
using Izumi.Services.Hangfire.BackgroundJobs.Unmute;
using Izumi.Services.Hangfire.Commands;
using MediatR;

namespace Izumi.Services.Discord.Commands.Prefix
{
    [RequireRole(DiscordRoleType.Moderator)]
    public class Mute : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public Mute(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("mute")]
        public async Task MuteTask(IUser mentionedUser, uint minutes, string reason = null)
        {
            var guildUser = await _mediator.Send(new GetSocketGuildUserQuery(mentionedUser.Id));
            var muted = await _mediator.Send(new CheckUserMutedQuery((long) mentionedUser.Id));

            var embed = new EmbedBuilder();

            if (muted)
            {
                embed.WithDescription($"Пользователь {guildUser.Mention} уже находится под блокировкой чата.");
            }
            else
            {
                await _mediator.Send(new AddRoleToGuildUserCommand(guildUser.Id, DiscordRoleType.Muted));
                await _mediator.Send(new CreateUserMuteCommand(
                    (long) guildUser.Id, (long) Context.User.Id, minutes, reason));

                var jobId = BackgroundJob.Schedule<IUnmuteJob>(
                    x => x.Execute((long) guildUser.Id),
                    TimeSpan.FromMinutes(minutes));

                await _mediator.Send(new CreateUserHangfireJobCommand(
                    (long) guildUser.Id, HangfireJobType.Unmute, jobId, DateTimeOffset.Now.AddMinutes(minutes)));

                embed.WithDescription($"Пользователю {guildUser.Mention} выдана блокировка чата.");

                var notify = new EmbedBuilder()
                    .WithDescription(
                        "Ты получил блокировку чата на " +
                        $"**{TimeSpan.FromMinutes(minutes).Humanize(1, new CultureInfo("ru-RU"))}**" +
                        (reason is null ? "." : $" по причине: **{reason}**."));

                await _mediator.Send(new SendEmbedToUserCommand(guildUser.Id, notify));
            }

            await _mediator.Send(new SendEmbedToUserCommand(Context.User.Id, embed));
        }
    }
}

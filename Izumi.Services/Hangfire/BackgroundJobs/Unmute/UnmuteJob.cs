using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.Unmute
{
    public class UnmuteJob : IUnmuteJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UnmuteJob> _logger;

        public UnmuteJob(
            IMediator mediator,
            ILogger<UnmuteJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(long userId)
        {
            _logger.LogInformation(
                "Unmute job executed for user {UserId}",
                userId);

            await _mediator.Send(new RemoveRoleFromGuildUserCommand((ulong) userId, DiscordRoleType.Muted));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireJobType.Unmute));

            var embed = new EmbedBuilder()
                .WithDescription("Действие блокировки чата закончилось.");

            await _mediator.Send(new SendEmbedToUserCommand((ulong) userId, embed));
        }
    }
}

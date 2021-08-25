using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Client.Events
{
    public record Ready(DiscordSocketClient SocketClient) : IRequest;

    public class ReadyHandler : IRequestHandler<Ready>
    {
        private readonly ILogger<ReadyHandler> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public ReadyHandler(
            ILogger<ReadyHandler> logger,
            IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _lifetime = lifetime;
        }

        public async Task<Unit> Handle(Ready request, CancellationToken cancellationToken)
        {
            await request.SocketClient.SetGameAsync("...", null, ActivityType.Watching);

            try
            {
                _logger.LogInformation("Bot started");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unable to startup the bot. Application will now exit");
                _lifetime.StopApplication();
            }

            return Unit.Value;
        }
    }
}

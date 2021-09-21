using System.Threading.Tasks;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Emote.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.UploadEmotes
{
    public class UploadEmotesJob : IUploadEmotesJob
    {
        private readonly IDiscordClientService _discordClientService;
        private readonly IMediator _mediator;
        private readonly ILogger<UploadEmotesJob> _logger;

        public UploadEmotesJob(
            IDiscordClientService discordClientService,
            IMediator mediator,
            ILogger<UploadEmotesJob> logger)
        {
            _discordClientService = discordClientService;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute()
        {
            _logger.LogInformation(
                "Upload emotes job executed");

            var socketClient = await _discordClientService.GetSocketClient();

            foreach (var guild in socketClient.Guilds)
            {
                foreach (var emote in guild.Emotes)
                {
                    try
                    {
                        await _mediator.Send(new CreateEmoteCommand((long) emote.Id, emote.Name, emote.ToString()));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
}

using System.Threading.Tasks;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Emote.Commands;
using MediatR;

namespace Izumi.Services.Hangfire.BackgroundJobs.UploadEmotes
{
    public class UploadEmotesJob : IUploadEmotesJob
    {
        private readonly IDiscordClientService _discordClientService;
        private readonly IMediator _mediator;

        public UploadEmotesJob(
            IDiscordClientService discordClientService,
            IMediator mediator)
        {
            _discordClientService = discordClientService;
            _mediator = mediator;
        }

        public async Task Execute()
        {
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

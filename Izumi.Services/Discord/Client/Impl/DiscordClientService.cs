using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Services.Discord.Client.Events;
using Izumi.Services.Discord.Client.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Izumi.Services.Discord.Client.Impl
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly IOptions<DiscordOptions> _options;
        private readonly IMediator _mediator;
        private readonly DiscordSocketClient _socketClient;

        public DiscordClientService(
            IOptions<DiscordOptions> options,
            IMediator mediator)
        {
            _options = options;
            _mediator = mediator;
            _socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
                AlwaysAcknowledgeInteractions = false,
                GatewayIntents =
                    GatewayIntents.Guilds |
                    GatewayIntents.GuildMembers |
                    GatewayIntents.GuildMessageReactions |
                    GatewayIntents.GuildMessages |
                    GatewayIntents.GuildVoiceStates
            });
        }

        public async Task<DiscordSocketClient> GetSocketClient()
        {
            return await Task.FromResult(_socketClient);
        }

        public async Task Start()
        {
            await _socketClient.LoginAsync(TokenType.Bot, _options.Value.Token);
            await _socketClient.StartAsync();

            _socketClient.Ready += SocketClientOnReady;
            _socketClient.InteractionCreated += SocketClientOnInteractionCreated;
        }

        private async Task SocketClientOnInteractionCreated(SocketInteraction interaction)
        {
            await _mediator.Send(new InteractionCreated(interaction));
        }

        private async Task SocketClientOnReady()
        {
            await _mediator.Send(new Ready(_socketClient));
        }
    }
}

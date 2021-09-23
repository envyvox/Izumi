using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
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
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _socketClient;

        public DiscordClientService(
            IOptions<DiscordOptions> options,
            IMediator mediator,
            CommandService commandService,
            IServiceProvider serviceProvider)
        {
            _options = options;
            _mediator = mediator;
            _commandService = commandService;
            _serviceProvider = serviceProvider;
            _socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
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
            await _commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            await _socketClient.LoginAsync(TokenType.Bot, _options.Value.Token);
            await _socketClient.StartAsync();

            _socketClient.Ready += SocketClientOnReady;
            _socketClient.InteractionCreated += SocketClientOnInteractionCreated;
            _socketClient.MessageReceived += SocketClientOnMessageReceived;
            _socketClient.MessageDeleted += SocketClientOnMessageDeleted;
            _socketClient.ReactionAdded += SocketClientOnReactionAdded;
            _socketClient.ReactionRemoved += SocketClientOnReactionRemoved;
            _socketClient.UserVoiceStateUpdated += SocketClientOnUserVoiceStateUpdated;
            _socketClient.UserJoined += SocketClientOnUserJoined;
            _commandService.CommandExecuted += CommandServiceOnCommandExecuted;
        }

        private async Task SocketClientOnUserJoined(SocketGuildUser socketGuildUser)
        {
            await _mediator.Send(new UserJoined(socketGuildUser));
        }

        private static async Task CommandServiceOnCommandExecuted(Optional<CommandInfo> command,
            ICommandContext context, IResult result)
        {
            if (!string.IsNullOrEmpty(result?.ErrorReason))
                await context.Channel.SendMessageAsync(context.User.Mention + ", " + result.ErrorReason);
        }

        private async Task SocketClientOnUserVoiceStateUpdated(SocketUser socketUser, SocketVoiceState oldVoiceState,
            SocketVoiceState newVoiceState)
        {
            await _mediator.Send(new UserVoiceStateUpdated(socketUser, oldVoiceState, newVoiceState));
        }

        private async Task SocketClientOnMessageDeleted(Cacheable<IMessage, ulong> message,
            Cacheable<IMessageChannel, ulong> channel)
        {
            await _mediator.Send(new MessageDeleted(message, channel));
        }

        private async Task SocketClientOnReactionRemoved(Cacheable<IUserMessage, ulong> message,
            Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            await _mediator.Send(new ReactionRemoved(message, channel, reaction));
        }

        private async Task SocketClientOnReactionAdded(Cacheable<IUserMessage, ulong> message,
            Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            await _mediator.Send(new ReactionAdded(message, channel, reaction));
        }

        private async Task SocketClientOnMessageReceived(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage message) return;
            if (message.Author.IsBot) return;

            await _mediator.Send(new MessageReceived(socketMessage));

            var argPos = 0;

            if (!message.HasMentionPrefix(_socketClient.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_socketClient, message);

            var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);

            if (result.IsSuccess)
            {
                await Task.Delay(1000);
                await message.DeleteAsync();
            }
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

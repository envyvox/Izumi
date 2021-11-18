using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Izumi.Services.Discord.Embed
{
    public record SendEmbedToUserCommand(
            ulong UserId,
            EmbedBuilder Builder,
            string Message = "")
        : IRequest;

    public class SendEmbedToUserHandler : IRequestHandler<SendEmbedToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SendEmbedToUserHandler> _logger;
        private readonly DiscordOptions _options;

        public SendEmbedToUserHandler(
            IMediator mediator,
            ILogger<SendEmbedToUserHandler> logger,
            IOptions<DiscordOptions> options)
        {
            _mediator = mediator;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<Unit> Handle(SendEmbedToUserCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.UserId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(request.UserId));
            var channels = DiscordRepository.Channels;

            var embed = request.Builder
                .WithUserColor(user.CommandColor)
                .Build();

            var component = new ComponentBuilder()
                .WithButton($"Открыть канал #{DiscordChannelType.Chat.Name()}", null, ButtonStyle.Link, null,
                    $"https://www.discord.com/channels/{_options.GuildId}/{channels[DiscordChannelType.Chat].Id}")
                .WithButton($"Открыть канал #{DiscordChannelType.Commands.Name()}", null, ButtonStyle.Link, null,
                    $"https://www.discord.com/channels/{_options.GuildId}/{channels[DiscordChannelType.Commands].Id}")
                .Build();

            try
            {
                await socketUser.SendMessageAsync(request.Message, embed: embed, component: component);

                _logger.LogInformation(
                    "Sended a direct message to user {UserId}",
                    request.UserId);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Can't send message to user {UserId}",
                    request.UserId);
            }

            return Unit.Value;
        }
    }
}
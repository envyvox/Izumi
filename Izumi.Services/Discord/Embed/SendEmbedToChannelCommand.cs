using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Embed
{
    public record SendEmbedToChannelCommand(
            DiscordChannelType Channel,
            EmbedBuilder Builder,
            string Message = "")
        : IRequest;

    public class SendEmbedToChannelHandler : IRequestHandler<SendEmbedToChannelCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SendEmbedToChannelHandler> _logger;

        public SendEmbedToChannelHandler(
            IMediator mediator,
            ILogger<SendEmbedToChannelHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(SendEmbedToChannelCommand request, CancellationToken ct)
        {
            var channels = DiscordRepository.Channels;
            var channel = await _mediator.Send(new GetSocketTextChannelQuery((ulong) channels[request.Channel].Id));

            try
            {
                await channel.SendMessageAsync(request.Message, embed: request.Builder.Build());

                _logger.LogInformation(
                    "Sended a message in channel {ChannelType}",
                    request.Channel.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Can't send message to channel {ChannelType}",
                    request.Channel.ToString());
            }

            return Unit.Value;
        }
    }
}

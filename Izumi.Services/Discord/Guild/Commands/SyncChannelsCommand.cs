using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Models;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Guild.Commands
{
    public record SyncChannelsCommand : IRequest;

    public class SyncChannelsHandler : IRequestHandler<SyncChannelsCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SyncChannelsHandler> _logger;

        public SyncChannelsHandler(
            IMediator mediator,
            ILogger<SyncChannelsHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(SyncChannelsCommand request, CancellationToken ct)
        {
            var channels = DiscordRepository.Channels;
            var channelTypes = Enum
                .GetValues(typeof(DiscordChannelType))
                .Cast<DiscordChannelType>()
                .ToArray();

            if (channels.Count < channelTypes.Length)
            {
                var guild = await _mediator.Send(new GetSocketGuildQuery());

                foreach (var channelType in channelTypes)
                {
                    if (channels.ContainsKey(channelType)) continue;

                    var chan = guild.Channels.FirstOrDefault(x => x.Name == channelType.Name());

                    if (chan is null)
                    {
                        switch (channelType.Category())
                        {
                            case DiscordChannelCategoryType.TextChannel:

                                var textChannel = await guild.CreateTextChannelAsync(channelType.Name(), x =>
                                {
                                    x.CategoryId = channels.ContainsKey(channelType.Parent())
                                        ? channels[channelType.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                channels.Add(channelType, new ChannelDto(textChannel.Id, channelType));

                                break;
                            case DiscordChannelCategoryType.VoiceChannel:

                                var voiceChannel = await guild.CreateVoiceChannelAsync(channelType.Name(), x =>
                                {
                                    x.CategoryId = channels.ContainsKey(channelType.Parent())
                                        ? channels[channelType.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                channels.Add(channelType, new ChannelDto(voiceChannel.Id, channelType));

                                break;
                            case DiscordChannelCategoryType.CategoryChannel:

                                var categoryChannel = await guild.CreateCategoryChannelAsync(channelType.Name());

                                channels.Add(channelType, new ChannelDto(categoryChannel.Id, channelType));

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        channels.Add(channelType, new ChannelDto(chan.Id, channelType));
                    }
                }
            }

            _logger.LogInformation(
                "Channels sync completed");

            return Unit.Value;
        }
    }
}

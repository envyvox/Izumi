using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Discord;
using Izumi.Data;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Enums.Discord;
using Izumi.Data.Extensions;
using Izumi.Services.Discord.Guild.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.Guild.Queries
{
    public record GetChannelsQuery : IRequest<Dictionary<DiscordChannelType, ChannelDto>>;

    public class GetChannelsHandler : IRequestHandler<GetChannelsQuery, Dictionary<DiscordChannelType, ChannelDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public GetChannelsHandler(
            DbContextOptions options,
            IMapper mapper,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Dictionary<DiscordChannelType, ChannelDto>> Handle(GetChannelsQuery request,
            CancellationToken ct)
        {
            var channels = await _db.Channels
                .AsQueryable()
                .ToDictionaryAsync(x => x.Type);

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
                                        ? (ulong) channels[channelType.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                channels.Add(channelType, new Channel { Id = (long) textChannel.Id, Type = channelType });
                                await _db.CreateEntity(new Channel { Id = (long) textChannel.Id, Type = channelType });

                                break;
                            case DiscordChannelCategoryType.VoiceChannel:

                                var voiceChannel = await guild.CreateVoiceChannelAsync(channelType.Name(), x =>
                                {
                                    x.CategoryId = channels.ContainsKey(channelType.Parent())
                                        ? (ulong) channels[channelType.Parent()].Id
                                        : Optional<ulong?>.Unspecified;
                                });

                                channels.Add(channelType, new Channel { Id = (long) voiceChannel.Id, Type = channelType });
                                await _db.CreateEntity(new Channel { Id = (long) voiceChannel.Id, Type = channelType });

                                break;
                            case DiscordChannelCategoryType.CategoryChannel:

                                var categoryChannel = await guild.CreateCategoryChannelAsync(channelType.Name());

                                channels.Add(channelType, new Channel { Id = (long) categoryChannel.Id, Type = channelType });
                                await _db.CreateEntity(new Channel { Id = (long) categoryChannel.Id, Type = channelType });

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        channels.Add(channelType, new Channel { Id = (long) chan.Id, Type = channelType });
                        await _db.CreateEntity(new Channel { Id = (long) chan.Id, Type = channelType });
                    }
                }
            }

            return _mapper.Map<Dictionary<DiscordChannelType, ChannelDto>>(channels);
        }
    }
}

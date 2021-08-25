using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Izumi.Services.Discord.Guild.Queries
{
    public record GetSocketGuildQuery(ulong GuildId = 0) : IRequest<SocketGuild>;

    public class GetSocketGuildHandler : IRequestHandler<GetSocketGuildQuery, SocketGuild>
    {
        private readonly IDiscordClientService _discordClientService;
        private readonly IOptions<DiscordOptions> _options;

        public GetSocketGuildHandler(
            IDiscordClientService discordClientService,
            IOptions<DiscordOptions> options)
        {
            _discordClientService = discordClientService;
            _options = options;
        }

        public async Task<SocketGuild> Handle(GetSocketGuildQuery request, CancellationToken ct)
        {
            var guildId = request.GuildId == 0
                ? _options.Value.GuildId
                : request.GuildId;
            var client = await _discordClientService.GetSocketClient();
            var guild = client.GetGuild(guildId);

            if (guild is null)
            {
                throw new Exception($"socket guild with id {guildId} was not found.");
            }

            return guild;
        }
    }
}

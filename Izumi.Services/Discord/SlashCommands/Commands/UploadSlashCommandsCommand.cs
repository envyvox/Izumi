using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Izumi.Data.Util;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Izumi.Services.Discord.SlashCommands.Commands
{
    public record UploadSlashCommandsCommand : IRequest<TotalAndAffectedCountDto>;

    public class UploadSlashCommandsHandler : IRequestHandler<UploadSlashCommandsCommand, TotalAndAffectedCountDto>
    {
        private readonly IDiscordClientService _discordClientService;
        private readonly IOptions<DiscordOptions> _options;

        public UploadSlashCommandsHandler(
            IDiscordClientService discordClientService,
            IOptions<DiscordOptions> options)
        {
            _discordClientService = discordClientService;
            _options = options;
        }

        public async Task<TotalAndAffectedCountDto> Handle(UploadSlashCommandsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var socketClient = await _discordClientService.GetSocketClient();
            var commands = new[]
            {
                new SlashCommandBuilder()
                    .WithName("ping")
                    .WithDescription("Отвечает Pong!"),
                new SlashCommandBuilder()
                    .WithName("профиль")
                    .WithDescription("Просмотр игрового профиля")
                    .AddOption("пользователь", ApplicationCommandOptionType.User,
                        "Пользователь, профиль которого ты хочешь посмотреть", false)
            };

            foreach (var command in commands)
            {
                result.Total++;

                try
                {
                    await socketClient.Rest.CreateGuildCommand(command.Build(), _options.Value.GuildId);

                    result.Affected++;
                }
                catch (ApplicationCommandException exception)
                {
                    var json = JsonConvert.SerializeObject(exception.Error, Formatting.Indented);
                    Console.WriteLine(json);
                }
            }

            return result;
        }
    }
}

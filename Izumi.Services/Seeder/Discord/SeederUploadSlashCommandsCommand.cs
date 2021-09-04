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

namespace Izumi.Services.Seeder.Discord
{
    public record SeederUploadSlashCommandsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadSlashCommandsHandler
        : IRequestHandler<SeederUploadSlashCommandsCommand, TotalAndAffectedCountDto>
    {
        private readonly IDiscordClientService _discordClientService;
        private readonly IOptions<DiscordOptions> _options;

        public SeederUploadSlashCommandsHandler(
            IDiscordClientService discordClientService,
            IOptions<DiscordOptions> options)
        {
            _discordClientService = discordClientService;
            _options = options;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadSlashCommandsCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var socketClient = await _discordClientService.GetSocketClient();
            var commands = new[]
            {
                new SlashCommandBuilder()
                    .WithName("доска-сообщества")
                    .WithDescription("Информация о твоем участии в доске сообщества"),

                new SlashCommandBuilder()
                    .WithName("профиль")
                    .WithDescription("Просмотр игрового профиля")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.User)
                        .WithName("пользователь")
                        .WithDescription("Пользователь, профиль которого ты хочешь посмотреть")
                        .WithRequired(false)),

                new SlashCommandBuilder()
                    .WithName("инвентарь")
                    .WithDescription("Просмотр игрового инвентаря")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithName("категория")
                        .WithDescription("Просмотр инвентаря определенной категории")
                        .WithRequired(false)
                        .AddChoice("рыба", "рыба")
                        .AddChoice("семена", "семена")
                        .AddChoice("урожай", "урожай")
                        .AddChoice("блюда", "блюда"))
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

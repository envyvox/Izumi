﻿using System;
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
                    .WithName("мир")
                    .WithDescription("Информация о состоянии игрового мира"),

                new SlashCommandBuilder()
                    .WithName("профиль")
                    .WithDescription("Просмотр игрового профиля")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.User)
                        .WithName("пользователь")
                        .WithDescription("Пользователь, профиль которого ты хочешь посмотреть")
                        .WithRequired(false)),

                new SlashCommandBuilder()
                    .WithName("информация")
                    .WithDescription("Обновление информации в профиле")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithName("текст")
                        .WithDescription("Новая информация")
                        .WithRequired(true)),

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
                        .AddChoice("блюда", "блюда")),

                new SlashCommandBuilder()
                    .WithName("титулы")
                    .WithDescription("Просмотр коллекции титулов"),

                new SlashCommandBuilder()
                    .WithName("титул")
                    .WithDescription("Обновление текущего титула")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithName("титул")
                        .WithDescription("Номер титула")
                        .WithRequired(true)),

                new SlashCommandBuilder()
                    .WithName("баннеры")
                    .WithDescription("Просмотр коллекции баннеров"),

                new SlashCommandBuilder()
                    .WithName("баннер")
                    .WithDescription("Обновление текущего титула")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithName("баннер")
                        .WithDescription("Номер баннера")
                        .WithRequired(true)),

                new SlashCommandBuilder()
                    .WithName("приглашения")
                    .WithDescription("Информация об участии в реферальной системе"),

                new SlashCommandBuilder()
                    .WithName("пригласил")
                    .WithDescription("Указать пользователя как пригласившего тебя")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.User)
                        .WithName("пользователь")
                        .WithDescription("Пользователь, который тебя пригласил")
                        .WithRequired(true))
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

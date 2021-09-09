using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Game.Localization;
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
        private readonly ILocalizationService _local;

        public SeederUploadSlashCommandsHandler(
            IDiscordClientService discordClientService,
            IOptions<DiscordOptions> options,
            ILocalizationService local)
        {
            _discordClientService = discordClientService;
            _options = options;
            _local = local;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadSlashCommandsCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var socketClient = await _discordClientService.GetSocketClient();
            var commands = new[]
            {
                new SlashCommandBuilder()
                    .WithName("ping")
                    .WithDescription("Respond with Pong"),

                new SlashCommandBuilder()
                    .WithName("пинг")
                    .WithDescription("Отвечает Понг"),

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
                    .WithName("коллекция")
                    .WithDescription("Просмотр своей коллекции")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithName("категория")
                        .WithDescription("Категория коллекции которую ты хочешь посмотреть")
                        .AddChoice(CollectionType.Gathering.Localize(), CollectionType.Gathering.GetHashCode())
                        .AddChoice(CollectionType.Crafting.Localize(), CollectionType.Crafting.GetHashCode())
                        .AddChoice(CollectionType.Alcohol.Localize(), CollectionType.Alcohol.GetHashCode())
                        .AddChoice(CollectionType.Drink.Localize(), CollectionType.Drink.GetHashCode())
                        .AddChoice(CollectionType.Crop.Localize(), CollectionType.Crop.GetHashCode())
                        .AddChoice(CollectionType.Fish.Localize(), CollectionType.Fish.GetHashCode())
                        .AddChoice(CollectionType.Food.Localize(), CollectionType.Food.GetHashCode())
                        .WithRequired(true)),

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
                        .WithRequired(true)),

                new SlashCommandBuilder()
                    .WithName("отправления")
                    .WithDescription("Просмотр доступных отправлений из текущей локации"),

                new SlashCommandBuilder()
                    .WithName("отправиться")
                    .WithDescription("Отправиться в указанную локацию")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithName("локация")
                        .WithDescription("Локация в которую ты хочешь отправиться")
                        .AddChoice(LocationType.Capital.Localize(), LocationType.Capital.GetHashCode())
                        .AddChoice(LocationType.Garden.Localize(), LocationType.Garden.GetHashCode())
                        .AddChoice(LocationType.Seaport.Localize(), LocationType.Seaport.GetHashCode())
                        .AddChoice(LocationType.Castle.Localize(), LocationType.Castle.GetHashCode())
                        .AddChoice(LocationType.Village.Localize(), LocationType.Village.GetHashCode())
                        .WithRequired(true)),

                new SlashCommandBuilder()
                    .WithName("исследовать")
                    .WithDescription($"Отправиться исследовать {LocationType.Garden.Localize()}"),

                new SlashCommandBuilder()
                    .WithName("копать")
                    .WithDescription($"Отправиться копать в {LocationType.Castle.Localize(true)}"),

                new SlashCommandBuilder()
                    .WithName("рыбачить")
                    .WithDescription($"Отправиться рыбачить в {LocationType.Seaport.Localize(true)}"),

                new SlashCommandBuilder()
                    .WithName("открыть")
                    .WithDescription("Открыть указанные коробки")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithName("количество")
                        .WithDescription("Количество коробок которое ты хочешь открыть")
                        .WithRequired(true))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithName("название")
                        .WithDescription("Название коробки которую ты хочешь открыть")
                        .AddChoice(_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString()),
                            BoxType.Capital.GetHashCode())
                        .AddChoice(_local.Localize(LocalizationCategoryType.Box, BoxType.Garden.ToString()),
                            BoxType.Garden.GetHashCode())
                        .AddChoice(_local.Localize(LocalizationCategoryType.Box, BoxType.Seaport.ToString()),
                            BoxType.Seaport.GetHashCode())
                        .AddChoice(_local.Localize(LocalizationCategoryType.Box, BoxType.Castle.ToString()),
                            BoxType.Castle.GetHashCode())
                        .AddChoice(_local.Localize(LocalizationCategoryType.Box, BoxType.Village.ToString()),
                            BoxType.Village.GetHashCode())
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

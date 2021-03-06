using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Game.Localization;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using IRequest = MediatR.IRequest;

namespace Izumi.Services.Seeder.Discord
{
    public record SeederUploadSlashCommandsCommand : IRequest;

    public class SeederUploadSlashCommandsHandler : IRequestHandler<SeederUploadSlashCommandsCommand>
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

        public async Task<Unit> Handle(SeederUploadSlashCommandsCommand request, CancellationToken ct)
        {
            var socketClient = await _discordClientService.GetSocketClient();

            ApplicationCommandProperties[] commands =
            {
                new SlashCommandBuilder()
                    .WithName("доска-сообщества")
                    .WithDescription("Информация о твоем участии в доске сообщества")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("мир")
                    .WithDescription("Информация о состоянии игрового мира")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("профиль")
                    .WithDescription("Просмотр игрового профиля")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.User)
                        .WithRequired(false)
                        .WithName("пользователь")
                        .WithDescription("Пользователь, профиль которого ты хочешь посмотреть"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("информация")
                    .WithDescription("Обновление информации в профиле")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("текст")
                        .WithDescription("Новая информация"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("инвентарь")
                    .WithDescription("Просмотр игрового инвентаря")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(false)
                        .WithName("категория")
                        .WithDescription("Просмотр инвентаря определенной категории")
                        .AddChoice("рыба", "рыба")
                        .AddChoice("семена", "семена")
                        .AddChoice("урожай", "урожай")
                        .AddChoice("блюда", "блюда"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("коллекция")
                    .WithDescription("Просмотр своей коллекции")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("категория")
                        .WithDescription("Категория коллекции которую ты хочешь посмотреть")
                        .AddChoice(CollectionType.Gathering.Localize(), CollectionType.Gathering.GetHashCode())
                        .AddChoice(CollectionType.Crafting.Localize(), CollectionType.Crafting.GetHashCode())
                        .AddChoice(CollectionType.Alcohol.Localize(), CollectionType.Alcohol.GetHashCode())
                        .AddChoice(CollectionType.Drink.Localize(), CollectionType.Drink.GetHashCode())
                        .AddChoice(CollectionType.Crop.Localize(), CollectionType.Crop.GetHashCode())
                        .AddChoice(CollectionType.Fish.Localize(), CollectionType.Fish.GetHashCode())
                        .AddChoice(CollectionType.Food.Localize(), CollectionType.Food.GetHashCode()))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("титулы")
                    .WithDescription("Просмотр коллекции титулов")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("титул")
                    .WithDescription("Обновление текущего титула")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("титул")
                        .WithDescription("Номер титула"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("баннеры")
                    .WithDescription("Просмотр коллекции баннеров")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("баннер")
                    .WithDescription("Обновление текущего титула")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("баннер")
                        .WithDescription("Номер баннера"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("приглашения")
                    .WithDescription("Информация об участии в реферальной системе")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("пригласил")
                    .WithDescription("Указать пользователя как пригласившего тебя")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.User)
                        .WithRequired(true)
                        .WithName("пользователь")
                        .WithDescription("Пользователь, который тебя пригласил"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("отправления")
                    .WithDescription("Просмотр доступных отправлений из текущей локации")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("отправиться")
                    .WithDescription("Отправиться в указанную локацию")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("локация")
                        .WithDescription("Локация в которую ты хочешь отправиться")
                        .AddChoice(LocationType.Capital.Localize(), LocationType.Capital.GetHashCode())
                        .AddChoice(LocationType.Garden.Localize(), LocationType.Garden.GetHashCode())
                        .AddChoice(LocationType.Seaport.Localize(), LocationType.Seaport.GetHashCode())
                        .AddChoice(LocationType.Castle.Localize(), LocationType.Castle.GetHashCode())
                        .AddChoice(LocationType.Village.Localize(), LocationType.Village.GetHashCode()))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("исследовать")
                    .WithDescription($"Отправиться исследовать {LocationType.Garden.Localize()}")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("копать")
                    .WithDescription($"Отправиться копать в {LocationType.Castle.Localize(true)}")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("рыбачить")
                    .WithDescription($"Отправиться рыбачить в {LocationType.Seaport.Localize(true)}")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("рыбак")
                    .WithDescription("Просмотр рыбы, которую покупает рыбак")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("рыбак-продать")
                    .WithDescription("Продать указанную рыбу")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("рыбу")
                        .WithDescription("Продать рыбаку определенную рыбу")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("номер")
                            .WithDescription("Номер рыбы, которую ты хочешь продать"))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("количество")
                            .WithDescription("Количество рыбы, которое ты хочешь продать")))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("все")
                        .WithDescription("Продать рыбаку всю подходяющую рыбу которая у тебя есть"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("открыть")
                    .WithDescription("Открыть указанные коробки")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("количество")
                        .WithDescription("Количество коробок которое ты хочешь открыть"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
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
                            BoxType.Village.GetHashCode()))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("магазин-посмотреть")
                    .WithDescription("Посмотреть товары магазина")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("тип")
                        .WithDescription("Тип товара, который ты хочешь посмотреть")
                        .AddChoice("семена", "seed")
                        .AddChoice("продукты", "product")
                        .AddChoice("рецепты", "recipe")
                        .AddChoice("баннеры", "banner"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("магазин-купить")
                    .WithDescription("Приобрести товар в магазине")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("тип")
                        .WithDescription("Тип товара, который ты хочешь приобрести")
                        .AddChoice("семена", "seed")
                        .AddChoice("продукт", "product")
                        .AddChoice("рецепт", "recipe")
                        .AddChoice("баннер", "banner"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("номер")
                        .WithDescription("Номер товара, который ты хочешь приобрести"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("количество")
                        .WithDescription("Количество товара, который ты хочешь приобрести"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("достижения")
                    .WithDescription("Просмотр своих достижений")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("категория")
                        .WithDescription("Категория достижений")
                        .AddChoice(AchievementCategoryType.FirstSteps.Localize(),
                            AchievementCategoryType.FirstSteps.GetHashCode())
                        .AddChoice(AchievementCategoryType.Gathering.Localize(),
                            AchievementCategoryType.Gathering.GetHashCode())
                        .AddChoice(AchievementCategoryType.Fishing.Localize(),
                            AchievementCategoryType.Fishing.GetHashCode())
                        .AddChoice(AchievementCategoryType.Harvesting.Localize(),
                            AchievementCategoryType.Harvesting.GetHashCode())
                        .AddChoice(AchievementCategoryType.Cooking.Localize(),
                            AchievementCategoryType.Cooking.GetHashCode())
                        .AddChoice(AchievementCategoryType.Crafting.Localize(),
                            AchievementCategoryType.Crafting.GetHashCode())
                        .AddChoice(AchievementCategoryType.Trading.Localize(),
                            AchievementCategoryType.Trading.GetHashCode())
                        .AddChoice(AchievementCategoryType.Casino.Localize(),
                            AchievementCategoryType.Casino.GetHashCode())
                        .AddChoice(AchievementCategoryType.Collection.Localize(),
                            AchievementCategoryType.Collection.GetHashCode())
                        .AddChoice(AchievementCategoryType.Event.Localize(),
                            AchievementCategoryType.Event.GetHashCode()))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("контракты")
                    .WithDescription("Просмотр доступных рабочих контрактов в текущей локации")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("съесть")
                    .WithDescription("Съесть указанное блюдо для восстановления энергии")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("количество")
                        .WithDescription("Количество которые ты хочешь съесть"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("название")
                        .WithDescription("Название блюда которое ты хочешь съесть")
                        .WithAutocomplete(true))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("репутация")
                    .WithDescription("Просмотр своей репутации в указанной локации")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("локация")
                        .WithDescription("Локация, репутацию которой ты хочешь посмотреть")
                        .AddChoice(ReputationType.Capital.Location().Localize(),
                            ReputationType.Capital.GetHashCode())
                        .AddChoice(ReputationType.Garden.Location().Localize(),
                            ReputationType.Garden.GetHashCode())
                        .AddChoice(ReputationType.Seaport.Location().Localize(),
                            ReputationType.Seaport.GetHashCode())
                        .AddChoice(ReputationType.Castle.Location().Localize(),
                            ReputationType.Castle.GetHashCode())
                        .AddChoice(ReputationType.Village.Location().Localize(),
                            ReputationType.Village.GetHashCode()))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("участок")
                    .WithDescription("Просмотр информации о своем участке земли")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("информация")
                        .WithDescription("Информация об участке земли"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("купить")
                        .WithDescription("Приобрести участок земли"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("посадить")
                        .WithDescription("Посадить семена на своем участке земли")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("номер")
                            .WithDescription("Номер клетки земли на которую ты хочешь посадить семена"))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.String)
                            .WithRequired(true)
                            .WithName("название")
                            .WithDescription("Название семян, которые ты хочешь посадить")
                            .WithAutocomplete(true)))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("полить")
                        .WithDescription("Полить семена на участке земли"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("собрать")
                        .WithDescription("Собрать урожай со своего участка земли")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("номер")
                            .WithDescription("Номер клетки земли с которой ты хочешь собрать урожай")))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("выкопать")
                        .WithDescription("Выкопать посаженные семена из своего участка земли")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("номер")
                            .WithDescription("Номер клетки земли которую ты хочешь освободить от семян")))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("изготовление")
                    .WithDescription("Посмотреть доступные для изготовления предметы")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("категория")
                        .WithDescription("Категория изготовления")
                        .AddChoice("предметов", "предметов")
                        .AddChoice("алкоголя", "алкоголя")
                        .AddChoice("напитков", "напитков"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("изготовить")
                    .WithDescription("Изготовить предмет, алкоголь или напиток")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("категория")
                        .WithDescription("Категория изготовления")
                        .AddChoice("предмет", "предмет")
                        .AddChoice("алкоголь", "алкоголь")
                        .AddChoice("напиток", "напиток"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("количество")
                        .WithDescription("Количество которое ты хочешь изготовить"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("название")
                        .WithDescription("Название предмета который ты хочешь изготовить")
                        .WithAutocomplete(true))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("приготовление")
                    .WithDescription("Посмотреть доступные для приготовления блюда")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("приготовить")
                    .WithDescription("Приготовить блюдо")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("количество")
                        .WithDescription("Количество которое ты хочешь приготовить"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("название")
                        .WithDescription("Название блюда которое ты хочешь приготовить")
                        .WithAutocomplete(true))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("лотерея")
                    .WithDescription("Просмотр или покупка лотерейного билета")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("информация")
                        .WithDescription("Просмотр информации о лотерее"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("купить")
                        .WithDescription("Приобрести лотерейный билет"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("подарить")
                        .WithDescription("Подарить лотерейный билет указанному пользователю")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.User)
                            .WithRequired(true)
                            .WithName("пользователь")
                            .WithDescription("Пользователь, которому ты хочешь подарить лотерейный билет")))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("ставка")
                    .WithDescription("Сделать ставку в казино")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("количество")
                        .WithDescription("Количество иен которые ты хочешь поставить"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("обучение")
                    .WithDescription("Начать или посмотреть свой текущий шаг обучения")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("рынок")
                    .WithDescription("Рынок")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithRequired(false)
                        .WithName("информация")
                        .WithDescription("Посмотреть информацию о товаре на рынке")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("категория")
                            .WithDescription("Категория товара который ты хочешь посмотреть")
                            .AddChoice(MarketCategoryType.Gathering.Localize(),
                                MarketCategoryType.Gathering.GetHashCode())
                            .AddChoice(MarketCategoryType.Crafting.Localize(),
                                MarketCategoryType.Crafting.GetHashCode())
                            .AddChoice(MarketCategoryType.Alcohol.Localize(),
                                MarketCategoryType.Alcohol.GetHashCode())
                            .AddChoice(MarketCategoryType.Drink.Localize(),
                                MarketCategoryType.Drink.GetHashCode())
                            .AddChoice(MarketCategoryType.Food.Localize(),
                                MarketCategoryType.Food.GetHashCode())
                            .AddChoice(MarketCategoryType.Crop.Localize(),
                                MarketCategoryType.Crop.GetHashCode()))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.String)
                            .WithRequired(true)
                            .WithName("название")
                            .WithDescription("Название товара который ты хочешь посмотреть")))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("купить")
                        .WithDescription("Приобрести товар с рынка")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("категория")
                            .WithDescription("Категория товара который ты хочешь приобрести")
                            .AddChoice(MarketCategoryType.Gathering.Localize(),
                                MarketCategoryType.Gathering.GetHashCode())
                            .AddChoice(MarketCategoryType.Crafting.Localize(),
                                MarketCategoryType.Crafting.GetHashCode())
                            .AddChoice(MarketCategoryType.Alcohol.Localize(),
                                MarketCategoryType.Alcohol.GetHashCode())
                            .AddChoice(MarketCategoryType.Drink.Localize(),
                                MarketCategoryType.Drink.GetHashCode())
                            .AddChoice(MarketCategoryType.Food.Localize(),
                                MarketCategoryType.Food.GetHashCode())
                            .AddChoice(MarketCategoryType.Crop.Localize(),
                                MarketCategoryType.Crop.GetHashCode()))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("количество")
                            .WithDescription("Количество которое ты хочешь приобрести"))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.String)
                            .WithRequired(true)
                            .WithName("название")
                            .WithDescription("Название товара который ты хочешь приобрести")))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("продать")
                        .WithDescription("Продать товар на рынок")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("категория")
                            .WithDescription("Категория товара который ты хочешь продать")
                            .AddChoice(MarketCategoryType.Gathering.Localize(),
                                MarketCategoryType.Gathering.GetHashCode())
                            .AddChoice(MarketCategoryType.Crafting.Localize(),
                                MarketCategoryType.Crafting.GetHashCode())
                            .AddChoice(MarketCategoryType.Alcohol.Localize(),
                                MarketCategoryType.Alcohol.GetHashCode())
                            .AddChoice(MarketCategoryType.Drink.Localize(),
                                MarketCategoryType.Drink.GetHashCode())
                            .AddChoice(MarketCategoryType.Food.Localize(),
                                MarketCategoryType.Food.GetHashCode())
                            .AddChoice(MarketCategoryType.Crop.Localize(),
                                MarketCategoryType.Crop.GetHashCode()))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("количество")
                            .WithDescription("Количество которое ты хочешь продать"))
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.String)
                            .WithRequired(true)
                            .WithName("название")
                            .WithDescription("Название товара который ты хочешь продать")))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("ежедневная-награда")
                    .WithDescription("Забрать свою ежедневную награду")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("настройка")
                    .WithDescription("Различные настройки игрового профиля")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("роли-титула")
                        .WithDescription("Переключение автоматической выдачи роли активного титула"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("цвета-команд")
                        .WithDescription("Настройка цвета левой полоски у команд (только для премиум пользователей)")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.String)
                            .WithRequired(true)
                            .WithName("цвет")
                            .WithDescription("HEX значение нового цвета полоски у команд")))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("передать")
                    .WithDescription("Передать указанному пользователю иены")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.User)
                        .WithRequired(true)
                        .WithName("пользователь")
                        .WithDescription("Пользователь которому ты хочешь передать иены"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.Integer)
                        .WithRequired(true)
                        .WithName("количество")
                        .WithDescription("Количествои иен которое ты хочешь передать"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("переименоваться")
                    .WithDescription("Изменить имя на сервере")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.String)
                        .WithRequired(true)
                        .WithName("имя")
                        .WithDescription(
                            "Новое имя (если новое имя будет нарушать правила сервере - мы его изменим без компенсаций)"))
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("активность")
                    .WithDescription("Просмотр информации о своей активности на сервере")
                    .Build()
            };

            try
            {
                await socketClient.Rest.BulkOverwriteGuildCommands(commands, _options.Value.GuildId);
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Error, Formatting.Indented);
                Console.WriteLine(json);
            }

            return Unit.Value;
        }
    }
}
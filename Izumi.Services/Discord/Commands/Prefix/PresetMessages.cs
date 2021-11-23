using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Options;
using static Discord.Emote;

namespace Izumi.Services.Discord.Commands.Prefix
{
    [RequireOwner]
    [Group("preset")]
    public class PresetMessages : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly IDiscordClientService _discordClientService;
        private readonly DiscordOptions _options;

        public PresetMessages(
            IMediator mediator,
            IDiscordClientService discordClientService,
            IOptions<DiscordOptions> options)
        {
            _mediator = mediator;
            _discordClientService = discordClientService;
            _options = options.Value;
        }

        [Command("welcome")]
        public async Task SendWelcomeMessageTask()
        {
            var channels = DiscordRepository.Channels;
            var roles = DiscordRepository.Roles;
            var emotes = DiscordRepository.Emotes;

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/912388119796543498/unknown.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithTitle("Навигатор по серверу")
                .WithDescription(
                    $"<#{channels[DiscordChannelType.Chat].Id}> - Общение на различные темы.\n" +
                    $"<#{channels[DiscordChannelType.Commands].Id}> - Взаимодействие с ботом.\n" +
                    $"<#{channels[DiscordChannelType.GetRoles].Id}> - Получение серверных ролей.\n" +
                    $"<#{channels[DiscordChannelType.Announcements].Id}> - Серверные оповещения.\n" +
                    "\n**Игровая вселенная**\n" +
                    $"<#{channels[DiscordChannelType.GameInfo].Id}> - Информация о боте.\n" +
                    $"<#{channels[DiscordChannelType.GameLore].Id}> - История игрового мира.\n" +
                    $"<#{channels[DiscordChannelType.GameEvents].Id}> - Оповещения об игровых событиях.\n" +
                    $"<#{channels[DiscordChannelType.GameUpdates].Id}> - Обновления бота.\n" +
                    "\n**Доска сообщества**\n" +
                    $"<#{channels[DiscordChannelType.CommunityDescHowItWork].Id}> - Описание того, как устроена доска.\n" +
                    $"<#{channels[DiscordChannelType.Photos].Id}> - Красивые ~~котики~~ фотографии.\n" +
                    $"<#{channels[DiscordChannelType.Screenshots].Id}> - Твои яркие моменты.\n" +
                    $"<#{channels[DiscordChannelType.Memes].Id}> - Говорящее само за себя название канала.\n" +
                    $"<#{channels[DiscordChannelType.Arts].Id}> - Красивые рисунки.\n" +
                    $"<#{channels[DiscordChannelType.Music].Id}> - Любимые треки или клипы.\n" +
                    $"<#{channels[DiscordChannelType.Erotic].Id}> - Изображения, носящие эротический характер.\n" +
                    $"<#{channels[DiscordChannelType.Nsfw].Id}> - Изображения 18+.\n" +
                    "\n**Великая «Тосёкан»**\n" +
                    $"<#{channels[DiscordChannelType.Rules].Id}> - Правила сервера.\n" +
                    $"<#{channels[DiscordChannelType.Giveaways].Id}> - Различные серверные розыгрыши.\n" +
                    $"<#{channels[DiscordChannelType.Suggestions].Id}> - Твои предложения по улучшению сервера или бота.\n" +
                    "\n**Голосовые каналы**\n" +
                    "На нашем сервере присутствует возможность **создавать** голосовые каналы самостоятельно, " +
                    $"для этого необходимо **зайти** в канал {emotes.GetEmote("DiscordVoiceChannel")} **Разжечь костер**.\n" +
                    $"{emotes.GetEmote("Arrow")} Ты можешь изменять название и все права созданной тобой комнаты.")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/912509699943968838/unknown.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithTitle("Мероприятия")
                .WithDescription(
                    "На нашем сервере проводятся различные мероприятия, такие как кастомные игры 5х5 в " +
                    "League of Legends, Valorant или сломанный телефон, мафия и другие.\n\n" +
                    "**Расписание мероприятий**\n" +
                    "Для того чтобы всегда быть в курсе всех мероприятий, нажми на кнопку " +
                    $"{emotes.GetEmote("DiscordEventSchedule")} **Меропрития** над текстовыми каналами, " +
                    "там отображаются все **запланированные** мероприятия.\n\n" +
                    "Выбери интересующее тебя мероприятие и нажми\n" +
                    $"{emotes.GetEmote("DiscordNotification")} **Интересует** для того чтобы получить уведомление о его " +
                    $"начале, а так же чтобы <@&{roles[DiscordRoleType.EventManager].Id}> было проще " +
                    "понять сколько людей собирается придти.")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/912503517435023380/unknown.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithTitle("Игровая вселенная")
                .WithDescription(
                    $"На сервере присутствует собственный RPG бот {Context.Client.CurrentUser.Mention}.\n\n" +
                    "Развитие персонажа, экономика, выращивание урожая, рыбалка, кулинария и многое другое " +
                    "ждет тебя в твоем приключении по игровому миру.\n\n" +
                    $"Заглядывай в <#{channels[DiscordChannelType.GameInfo].Id}> чтобы узнать подробнее.")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/912516229015674900/unknown.png")
                .Build());
        }

        [Command("game-roles")]
        public async Task SendGameRolesMessageTask()
        {
            var emotes = DiscordRepository.Emotes;

            var embed = new EmbedBuilder()
                .WithDefaultColor()
                .WithAuthor("Игровые роли")
                .WithDescription(
                    "Ты можешь получить **игровые роли**, которые открывают доступ к **текстовым каналам** где можно " +
                    "как **найти людей** для совместной игры, так и просто пообщаться на игровую тематику. " +
                    "Для этого **выбери роли** из списка под этим сообщением.")
                .WithFooter("Игровые роли можно при необходимости снять, убрав их из списка.")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.GetGameRoles)));

            var menu = new ComponentBuilder()
                .WithSelectMenu(new SelectMenuBuilder()
                    .WithPlaceholder("Выберите игровые роли")
                    .WithCustomId("select-game-roles")
                    .WithMinValues(0)
                    .WithMaxValues(14)
                    .AddOption("Genshin Impact", "GenshinImpact", emote: Parse(emotes.GetEmote("GenshinImpact")))
                    .AddOption("League of Legends", "LeagueOfLegends", emote: Parse(emotes.GetEmote("LeagueOfLegends")))
                    .AddOption("Teamfight Tactics", "TeamfightTactics",
                        emote: Parse(emotes.GetEmote("TeamfightTactics")))
                    .AddOption("Valorant", "Valorant", emote: Parse(emotes.GetEmote("Valorant")))
                    .AddOption("Apex Legends", "ApexLegends", emote: Parse(emotes.GetEmote("ApexLegends")))
                    .AddOption("Dota 2", "Dota", emote: Parse(emotes.GetEmote("Dota")))
                    .AddOption("Minecraft", "Minecraft", emote: Parse(emotes.GetEmote("Minecraft")))
                    .AddOption("Among Us", "AmongUs", emote: Parse(emotes.GetEmote("AmongUs")))
                    .AddOption("Osu", "Osu", emote: Parse(emotes.GetEmote("Osu")))
                    .AddOption("Rust", "Rust", emote: Parse(emotes.GetEmote("Rust")))
                    .AddOption("CS:GO", "CSGO", emote: Parse(emotes.GetEmote("CSGO")))
                    .AddOption("HotS", "HotS", emote: Parse(emotes.GetEmote("HotS")))
                    .AddOption("New World", "NewWorld", emote: Parse(emotes.GetEmote("NewWorld")))
                    .AddOption("Mobile Gaming", "MobileGaming", emote: Parse(emotes.GetEmote("MobileGaming"))));

            await Context.Channel.SendMessageAsync(embed: embed.Build(), component: menu.Build());
        }

        [Command("gender-role")]
        public async Task SendGenderRoleMessageTask()
        {
            var emotes = DiscordRepository.Emotes;
            var roles = DiscordRepository.Roles;

            var embed = new EmbedBuilder()
                .WithDefaultColor()
                .WithAuthor("Подтверждение пола")
                .WithDescription(
                    "Ты можешь запросить **подтверждение пола** и получить роль " +
                    $"<@&{roles[DiscordRoleType.GenderMale].Id}> или <@&{roles[DiscordRoleType.GenderFemale].Id}>, " +
                    "открывающую доступ к особому **голосовому каналу**, доступному только пользователям с этим полом, а так же отображение пола " +
                    $"{emotes.GetEmote(GenderType.Male.EmoteName())}{emotes.GetEmote(GenderType.Female.EmoteName())} " +
                    "в **игровом профиле**. Для этого **нажми на кнопку** под этим сообщением.")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.RequestGenderRole)))
                .WithFooter("Гендерную роль можно при необходимости снять, попросив об этом администратора.");

            var button = new ComponentBuilder()
                .WithButton("Запросить роль", "request-gender-role");

            await Context.Channel.SendMessageAsync(embed: embed.Build(), component: button.Build());
        }

        [Command("how-desc-work")]
        public async Task SendHowCommunityDescWorkMessageTask()
        {
            var emotes = DiscordRepository.Emotes;
            var channels = DiscordRepository.Channels;
            var roles = DiscordRepository.Roles;

            var embed = new EmbedBuilder()
                .WithDefaultColor()
                .WithAuthor("Доска сообщества")
                .WithDescription(
                    "Ты можешь делиться с нами своими любимыми изображениями в каналах доски сообщества." +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши в канал <#{channels[DiscordChannelType.Commands].Id}> " +
                    "`/доска-сообщества` чтобы посмотреть информацию о своем участии." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Каналы доски",
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Photos].Id}> - Красивые ~~котики~~ фотографии.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Screenshots].Id}> - Твои яркие моменты.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Memes].Id}> - Говорящее само за себя название канала.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Arts].Id}> - Красивые рисунки.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Music].Id}> - Твоя любимая музыка.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Erotic].Id}> - Изображения, носящие эротический характер.\n" +
                    $"{emotes.GetEmote("List")} <#{channels[DiscordChannelType.Nsfw].Id}> - Изображения 18+." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Получение роли",
                    $"Пользователи, публикации который набирают суммарно {emotes.GetEmote("Like")} 500 лайков " +
                    $"получают роль <@&{roles[DiscordRoleType.ContentProvider].Id}> на 30 дней." +
                    $"\n\n{emotes.GetEmote("Arrow")} Если пользователь получит еще {emotes.GetEmote("Like")} 500 лайков " +
                    "уже имея роль, то ее длительность увеличится на 30 дней." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Модерация",
                    $"Публикации набирающие {emotes.GetEmote("Dislike")} 5 дизлайков будут автоматически удалены." +
                    $"\n\n{emotes.GetEmote("Arrow")} Публикации нарушающие правила сервера или правила отдельных " +
                    "каналов будут удалены модерацией сервера без предупреждения.");

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("game-info")]
        public async Task SendGameInfoTask()
        {
            var client = await _discordClientService.GetSocketClient();
            var emotes = DiscordRepository.Emotes;
            var channels = DiscordRepository.Channels;

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"На нашем сервере присутствует собственный бот {client.CurrentUser.Mention}" +
                    "\nс упрощенной RPG системой." +
                    $"\n\n{emotes.GetEmote("Arrow")} Все игровые команды отправляются в текстовый чат <#{channels[DiscordChannelType.Commands].Id}>.")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    "Мы **рекомендуем** начать знакомство с игровым миром с прохождения `/обучение`.")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/899923888224468992/6b30851aad3530d0.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription("В процессе прохождения **обучения** вы узнаете про:")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"{emotes.GetEmote("List")} **Исследования** локаций, добывая различные " +
                    $"{emotes.GetEmote("Flax")}{emotes.GetEmote("Wood")}{emotes.GetEmote("Gold")}{emotes.GetEmote("Coal")} ресурсы;")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/899925204036362241/b77fb0bfce1c814f.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"{emotes.GetEmote("List")} **Рыбалку**, где можно поймать различную " +
                    $"{emotes.GetEmote("Octopus")} рыбу на {emotes.GetEmote("Ien")} продажу или переработку;")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/899925662461206549/3e6494245538e61b.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"{emotes.GetEmote("List")} Разнообразные **магазины** для приобретения " +
                    $"{emotes.GetEmote("BlueberrySeeds")}{emotes.GetEmote("GrapeSeeds")} семян, {emotes.GetEmote("Egg")} продуктов, " +
                    $"{emotes.GetEmote("Recipe")} рецептов блюд, баннеров для профиля и т.д.;")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/899923884567068692/ec6b7bcffbf1a1d3.gif")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"{emotes.GetEmote("List")} **Участок**, где можно выращивать " +
                    $"{emotes.GetEmote("Blueberry")}{emotes.GetEmote("Grape")} урожай для приготовления " +
                    $"{emotes.GetEmote("BlueberryTart")} блюд или же просто для продажи на рынке;")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/899923906142556190/cd952a99a3ff5b23.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"{emotes.GetEmote("List")} **Казино** и {emotes.GetEmote("LotteryTicket")} лотерею;")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/899923901474283560/5932a15bff178d2f.png")
                .Build());

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"{emotes.GetEmote("List")} **Репутацию** городов, которую можно повысить за счет " +
                    "выполнения рабочих контрактов и победы над ежедневным боссом;")
                .WithImageUrl(
                    "https://cdn.discordapp.com/attachments/842067362139209778/899923892192296980/ae537eb1e5dc6b37.png")
                .Build());
        }
    }
}
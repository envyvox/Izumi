using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Commands.Component;
using Izumi.Services.Discord.Commands.Slash;
using Izumi.Services.Discord.Commands.Slash.Casino;
using Izumi.Services.Discord.Commands.Slash.Contract;
using Izumi.Services.Discord.Commands.Slash.Cooking;
using Izumi.Services.Discord.Commands.Slash.Crafting;
using Izumi.Services.Discord.Commands.Slash.Explore;
using Izumi.Services.Discord.Commands.Slash.Field;
using Izumi.Services.Discord.Commands.Slash.Fisher;
using Izumi.Services.Discord.Commands.Slash.Info;
using Izumi.Services.Discord.Commands.Slash.Info.Interaction;
using Izumi.Services.Discord.Commands.Slash.Market;
using Izumi.Services.Discord.Commands.Slash.Referral;
using Izumi.Services.Discord.Commands.Slash.Settings;
using Izumi.Services.Discord.Commands.Slash.Shop;
using Izumi.Services.Discord.Commands.Slash.Transit;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using static Izumi.Services.Extensions.ExceptionExtensions;


namespace Izumi.Services.Discord.Client.Events
{
    public record InteractionCreated(SocketInteraction Interaction) : IRequest;

    public class InteractionCreatedHandler : IRequestHandler<InteractionCreated>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InteractionCreatedHandler> _logger;

        public InteractionCreatedHandler(
            IMediator mediator,
            ILogger<InteractionCreatedHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(InteractionCreated request, CancellationToken ct)
        {
            try
            {
                switch (request.Interaction)
                {
                    case SocketSlashCommand command:
                        return command.Data.Name switch
                        {
                            "доска-сообщества" =>
                                await HandleInteraction(request.Interaction, new CommunityDescCommand(command), true),
                            "мир" =>
                                await HandleInteraction(request.Interaction, new WorldInfoCommand(command), true),
                            "профиль" =>
                                await HandleInteraction(request.Interaction, new ProfileCommand(command), true),
                            "информация" =>
                                await HandleInteraction(request.Interaction, new UpdateAboutCommand(command), false),
                            "инвентарь" =>
                                await HandleInteraction(request.Interaction, new InventoryCommand(command), true),
                            "коллекция" =>
                                await HandleInteraction(request.Interaction, new CollectionCommand(command), true),
                            "титулы" =>
                                await HandleInteraction(request.Interaction, new TitlesCommand(command), true),
                            "титул" =>
                                await HandleInteraction(request.Interaction, new UpdateTitleCommand(command), false),
                            "баннеры" =>
                                await HandleInteraction(request.Interaction, new BannersCommand(command), true),
                            "баннер" =>
                                await HandleInteraction(request.Interaction, new UpdateBannerCommand(command), false),
                            "приглашения" =>
                                await HandleInteraction(request.Interaction, new ReferralListCommand(command), true),
                            "пригласил" =>
                                await HandleInteraction(request.Interaction, new ReferralSetCommand(command), false),
                            "отправления" =>
                                await HandleInteraction(request.Interaction, new TransitListCommand(command), true),
                            "отправиться" =>
                                await HandleInteraction(request.Interaction, new TransitMakeCommand(command), false),
                            "исследовать" =>
                                await HandleInteraction(request.Interaction, new ExploreGardenCommand(command), false),
                            "копать" =>
                                await HandleInteraction(request.Interaction, new ExploreCastleCommand(command), false),
                            "рыбачить" =>
                                await HandleInteraction(request.Interaction, new FishingCommand(command), false),
                            "рыбак" =>
                                await HandleInteraction(request.Interaction, new FisherListCommand(command), true),
                            "рыбак-продать" =>
                                await HandleInteraction(request.Interaction, new FisherSellCommand(command), false),
                            "открыть" =>
                                await HandleInteraction(request.Interaction, new OpenBoxCommand(command), false),
                            "магазин-посмотреть" =>
                                await HandleInteraction(request.Interaction, new ShopListCommand(command), true),
                            "магазин-купить" =>
                                await HandleInteraction(request.Interaction, new ShopBuyCommand(command), false),
                            "контракты" =>
                                await HandleInteraction(request.Interaction, new ContractListCommand(command), true),
                            "контракт" =>
                                await HandleInteraction(request.Interaction, new ContractAcceptCommand(command), false),
                            "съесть" =>
                                await HandleInteraction(request.Interaction, new EatFoodCommand(command), false),
                            "репутация" =>
                                await HandleInteraction(request.Interaction, new ReputationsCommand(command), true),
                            "участок" =>
                                await HandleInteraction(request.Interaction, new FieldCommand(command), true),
                            "изготовление" =>
                                await HandleInteraction(request.Interaction, new CraftingListCommand(command), true),
                            "изготовить" =>
                                await HandleInteraction(request.Interaction, new CraftingStartCommand(command), false),
                            "приготовление" =>
                                await HandleInteraction(request.Interaction, new CookingListCommand(command), true),
                            "приготовить" =>
                                await HandleInteraction(request.Interaction, new CookingStartCommand(command), false),
                            "лотерея" =>
                                await HandleInteraction(request.Interaction, new LotteryCommand(command), true),
                            "ставка" =>
                                await HandleInteraction(request.Interaction, new BetCommand(command), false),
                            "обучение" =>
                                await HandleInteraction(request.Interaction, new TutorialCommand(command), true),
                            "рынок" =>
                                await HandleInteraction(request.Interaction, new MarketCommand(command), true),
                            "достижения" =>
                                await HandleInteraction(request.Interaction, new AchievementsCommand(command), true),
                            "ежедневная-награда" =>
                                await HandleInteraction(request.Interaction, new DailyRewardCommand(command), false),
                            "настройка" =>
                                await HandleInteraction(request.Interaction, new SettingsCommand(command), true),
                            _ => Unit.Value
                        };
                    case SocketMessageComponent component:

                        if (component.Data.CustomId.Contains("toggle-role"))
                            await HandleInteraction(request.Interaction, new ToggleRoleButton(component), true);
                        if (component.Data.CustomId == "select-game-roles")
                            await HandleInteraction(request.Interaction, new SelectGameRolesMenu(component), true);

                        break;
                }
            }
            catch (GameUserExpectedException e)
            {
                var emotes = await _mediator.Send(new GetEmotesQuery());
                var user = await _mediator.Send(new GetUserQuery((long) request.Interaction.User.Id));

                var embed = new EmbedBuilder()
                    .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)))
                    .WithAuthor("Ой, кажется что-то пошло не так...")
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} " +
                        $"{request.Interaction.User.Mention}, {e.Message}")
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.CommandError)));

                await request.Interaction.FollowupAsync("", new[] { embed.Build() });
            }
            catch (Exception e)
            {
                var emotes = await _mediator.Send(new GetEmotesQuery());
                var user = await _mediator.Send(new GetUserQuery((long) request.Interaction.User.Id));

                var embed = new EmbedBuilder()
                    .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)))
                    .WithAuthor("Ой, кажется что-то пошло не так...")
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} " +
                        $"{request.Interaction.User.Mention}, произошло что-то необычное и я уже сообщила об " +
                        "этом команде разработки. Приношу извинения за моих глупых создателей, они обязательно исправятся.")
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.CommandError)));

                await request.Interaction.FollowupAsync("", new[] { embed.Build() }, ephemeral: true);

                _logger.LogError(e, "Interaction ended with unexpected exception");
            }

            return Unit.Value;
        }

        private async Task<Unit> HandleInteraction<T>(SocketInteraction interaction, T implementation, bool ephemeral)
            where T : IRequest
        {
            await interaction.DeferAsync(ephemeral, new RequestOptions
            {
                RetryMode = RetryMode.Retry502,
                Timeout = 10000
            });

            switch (interaction)
            {
                case SocketSlashCommand command:

                    _logger.LogInformation(
                        "{UserName} {UserId} executed slash command /{CommandName} with options: {Options}",
                        command.User.Username, command.User.Id, command.Data.Name, command.Data.Options?
                            .Aggregate(string.Empty, (s, v) => s + $"{v.Name}: {v.Value}, ")
                            .RemoveFromEnd(2));

                    break;

                case SocketMessageComponent component:

                    _logger.LogInformation(
                        "{UserName} {UserId} used a component with id {ButtonId}",
                        component.User.Username, component.User.Id, component.Data.CustomId);

                    break;
            }

            return await _mediator.Send(implementation);
        }
    }
}

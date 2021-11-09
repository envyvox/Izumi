using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Collection.Commands;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Food.Commands;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Ingredient.Commands;
using Izumi.Services.Game.Ingredient.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Localization.Queries;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Cooking
{
    public record CookingStartCommand(SocketSlashCommand Command) : IRequest;

    public class CookingStartHandler : IRequestHandler<CookingStartCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CookingStartHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CookingStartCommand request, CancellationToken ct)
        {
            var amount = (uint) (long) request.Command.Data.Options
                .Single(x => x.Name == "количество").Value;
            var name = (string) request.Command.Data.Options
                .Single(x => x.Name == "название").Value;

            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Garden);

            var emotes = DiscordRepository.Emotes;
            var foodLocal = await _mediator.Send(new GetLocalizationByLocalizedNameQuery(
                LocalizationCategoryType.Food, name));
            var food = await _mediator.Send(new GetFoodByNameQuery(foodLocal.Name));
            var hasRecipe = await _mediator.Send(new CheckUserHasRecipeQuery(user.Id, food.Id));

            var embed = new EmbedBuilder()
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Cooking)));

            if (hasRecipe is false)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"для приготовления {emotes.GetEmote(food.Name)} " +
                    $"{_local.Localize(LocalizationCategoryType.Food, food.Name, 5)} необходимо сначала получить " +
                    $"{emotes.GetEmote("Recipe")} рецепт.");
            }
            else
            {
                var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(food.Ingredients));
                var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice, amount));
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

                if (userCurrency.Amount < craftingPrice)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 5)} " +
                        "для оплаты приготовления.");
                }
                else
                {
                    var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.EnergyCostMaking));
                    var ingredientsString = await _mediator.Send(new GetFoodIngredientsStringQuery(
                        food.Ingredients, amount));

                    await _mediator.Send(new CheckFoodIngredientsInUserCommand(user.Id, food.Ingredients, amount));
                    await _mediator.Send(new RemoveFoodIngredientsFromUserCommand(user.Id, food.Ingredients, amount));
                    await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, craftingPrice));
                    await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost * amount));
                    await _mediator.Send(new AddFoodToUserCommand(user.Id, food.Id, amount));
                    await _mediator.Send(new AddCollectionToUserCommand(user.Id, CollectionType.Food, food.Id));
                    await _mediator.Send(new AddStatisticToUserCommand(user.Id, StatisticType.Cooking, amount));
                    await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                    {
                        AchievementType.FirstCook,
                        AchievementType.CompleteCollectionFood
                    }));

                    embed
                        .WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"собрав все необходимые ингредиенты и сверившись с {emotes.GetEmote("Recipe")} рецептом, " +
                            $"ты успешно приготовил {emotes.GetEmote(food.Name)} {amount} " +
                            $"{_local.Localize(LocalizationCategoryType.Food, food.Name, amount)}." +
                            $"\n{StringExtensions.EmptyChar}")
                        .AddField("Затраченные ингредиенты",
                            ingredientsString +
                            $", {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}")
                        .AddField("Расход энергии",
                            $"{emotes.GetEmote("Energy")} {energyCost * amount} " +
                            $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost * amount)}");

                    var tutorialEatFoodIncId = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.TutorialEatFoodIncId));

                    if (food.AutoIncrementedId == tutorialEatFoodIncId)
                        await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CookFriedEgg));
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
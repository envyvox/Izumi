using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Food.Commands;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Component
{
    public record ShopBuyRecipeMenu(SocketMessageComponent Component) : IRequest;

    public class ShopBuyRecipeMenuHandler : IRequestHandler<ShopBuyRecipeMenu>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuyRecipeMenuHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopBuyRecipeMenu request, CancellationToken ct)
        {
            var foodIncId = long.Parse(request.Component.Data.Values.First());

            var user = await _mediator.Send(new GetUserQuery((long) request.Component.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Garden);

            var emotes = DiscordRepository.Emotes;
            var food = await _mediator.Send(new GetFoodByIncIdQuery(foodIncId));

            var hasRecipe = await _mediator.Send(new CheckUserHasRecipeQuery(user.Id, food.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor("Магазин рецептов")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopRecipe)));

            if (hasRecipe)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                    $"у тебя уже есть {emotes.GetEmote("Recipe")} рецепт " +
                    $"{_local.Localize(LocalizationCategoryType.Food, food.Name, 2)}.");
            }
            else
            {
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));
                var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(food.Ingredients));
                var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                var recipePrice = await _mediator.Send(new GetFoodRecipePriceQuery(costPrice, craftingPrice));

                if (userCurrency.Amount < recipePrice)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                        $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 5)} " +
                        $"для приобретения {emotes.GetEmote("Recipe")} рецепта " +
                        $"{_local.Localize(LocalizationCategoryType.Food, food.Name, 2)}.");
                }
                else
                {
                    await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, recipePrice));
                    await _mediator.Send(new CreateUserRecipeCommand(user.Id, food.Id));

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                        $"ты успешно приобрел {emotes.GetEmote("Recipe")} рецепт " +
                        $"{_local.Localize(LocalizationCategoryType.Food, food.Name, 2)} за " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {recipePrice} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), recipePrice)}.");
                }
            }

            await request.Component.FollowupAsync(embed: embed.Build());

            return Unit.Value;
        }
    }
}
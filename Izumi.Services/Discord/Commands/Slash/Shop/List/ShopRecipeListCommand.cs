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
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Izumi.Services.Discord.Commands.Slash.Shop.List
{
    public record ShopRecipeListCommand(SocketSlashCommand Command) : IRequest;

    public class ShopRecipeListHandler : IRequestHandler<ShopRecipeListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopRecipeListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopRecipeListCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Garden);

            var emotes = DiscordRepository.Emotes;
            var recipes = await _mediator.Send(new GetFoodsQuery());

            recipes = recipes
                .Where(x => x.RecipeSellable is true)
                .Take(14)
                .ToList();

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин рецептов")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются рецепты:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Переключай страницы магазины, нажимая **Назад** или **Вперед** под этим сообщением." +
                    $"\n\n{emotes.GetEmote("Arrow")} Для приобретения рецепта, **выбери его** из списка под этим сообщением." +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopRecipe)))
                .WithFooter("Страница 1");

            var component = new ComponentBuilder()
                .WithButton("Назад", "shop-recipe-back", disabled: true)
                .WithButton("Вперед", "shop-recipe-forward");

            var selectMenu = new SelectMenuBuilder()
                .WithCustomId("shop-buy-recipe")
                .WithPlaceholder("Выбери рецепт который хочешь приобрести");

            var counter = 0;
            foreach (var recipe in recipes)
            {
                counter++;

                var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(recipe.Ingredients));
                var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                var recipePrice = await _mediator.Send(new GetFoodRecipePriceQuery(costPrice, craftingPrice));

                embed.AddField(
                    $"{emotes.GetEmote("Recipe")} {_local.Localize(LocalizationCategoryType.Food, recipe.Name)}",
                    $"\nСтоимость: {emotes.GetEmote(CurrencyType.Ien.ToString())} {recipePrice} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), recipePrice)}",
                    true);

                if (counter == 2)
                {
                    counter = 0;
                    embed.AddEmptyField(true);
                }

                selectMenu.AddOption(
                    _local.Localize(LocalizationCategoryType.Food, recipe.Name),
                    $"{recipe.AutoIncrementedId}",
                    emote: Parse(emotes.GetEmote("Recipe")));
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed,
                component.WithSelectMenu(selectMenu)));
        }
    }
}
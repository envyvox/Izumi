using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Izumi.Services.Discord.Commands.Component
{
    public record ShopRecipePaginatorButton(SocketMessageComponent Component, bool IsForward) : IRequest;
    
    public class ShopRecipePaginatorButtonHandler : IRequestHandler<ShopRecipePaginatorButton>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopRecipePaginatorButtonHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopRecipePaginatorButton request, CancellationToken ct)
        {
            var messageEmbed = request.Component.Message.Embeds.First();
            var currentPage = int.Parse(Regex.Match(messageEmbed.Footer?.Text!, @"\d+").Value);
            var newPage = request.IsForward ? currentPage + 1 : currentPage - 1;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Component.User.Id));
            var recipes = await _mediator.Send(new GetFoodsQuery());
            var maxPages = recipes.Count / 14;
            
            recipes = recipes
                .Skip(newPage > 1 ? newPage * 14 : 0)
                .Take(14)
                .ToList();
            
            var embed = new EmbedBuilder()
                .WithAuthor("Магазин рецептов")
                .WithUserColor(user.CommandColor)
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                    "тут отображаются рецепты:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Переключай страницы магазины, нажимая **Назад** или **Вперед** под этим сообщением." +
                    $"\n\n{emotes.GetEmote("Arrow")} Для приобретения рецепта, **выбери его** из списка под этим сообщением." +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopRecipe)))
                .WithFooter($"Страница {newPage}");
            
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
            
            await request.Component.ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = new ComponentBuilder()
                    .WithButton("Назад", "shop-recipe-back", disabled: newPage <= 1)
                    .WithButton("Вперед", "shop-recipe-forward", disabled: newPage >= maxPages)
                    .WithSelectMenu(selectMenu)
                    .Build();
            });

            return Unit.Value;
        }
    }
}
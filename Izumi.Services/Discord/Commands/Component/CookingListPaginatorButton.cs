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
using Izumi.Services.Game.Ingredient.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Component
{
    public record CookingListPaginatorButton(SocketMessageComponent Component, bool IsForward) : IRequest;

    public class CookingListPaginatorButtonHandler : IRequestHandler<CookingListPaginatorButton>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CookingListPaginatorButtonHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CookingListPaginatorButton request, CancellationToken ct)
        {
            var messageEmbed = request.Component.Message.Embeds.First();
            var currentPage = int.Parse(Regex.Match(messageEmbed.Footer?.Text!, @"\d+").Value);
            var newPage = request.IsForward ? currentPage + 1 : currentPage - 1;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Component.User.Id));
            var foods = await _mediator.Send(new GetFoodsQuery());
            var maxPages = foods.Count / 5;

            foods = foods
                .Skip(newPage > 1 ? newPage * 5 : 0)
                .Take(5)
                .ToList();

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)))
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                    "тут отображаются всевозможные рецепты:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Переключай страницы списка рецептов нажимая **Назад** или **Вперед** под этим сообщением." +
                    $"\n\n{emotes.GetEmote("Arrow")} Для приготовления напиши `/приготовить` и укажи количество и название блюда." +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Cooking)))
                .WithFooter($"Страница {newPage}");

            foreach (var food in foods)
            {
                var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(food.Ingredients));
                var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                var energyRecharge = await _mediator.Send(new GetFoodEnergyRechargeQuery(costPrice, craftingPrice));
                var ingredientsString = await _mediator.Send(new GetFoodIngredientsStringQuery(food.Ingredients));
                var hasRecipe = await _mediator.Send(new CheckUserHasRecipeQuery(user.Id, food.Id));

                embed.AddField(
                    $"{emotes.GetEmote(hasRecipe ? "Checkmark" : "Crossmark")} {emotes.GetEmote(food.Name)} " +
                    $"{_local.Localize(LocalizationCategoryType.Food, food.Name)}",
                    $"Ингредиенты: {ingredientsString}" +
                    $"\nСтоимость приготовления: {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}" +
                    $"\nВосстановление энергии: {emotes.GetEmote("Energy")} {energyRecharge} {_local.Localize(LocalizationCategoryType.Bar, "Energy", energyRecharge)}");
            }

            await request.Component.ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = new ComponentBuilder()
                    .WithButton("Назад", "cooking-list-back", disabled: newPage <= 1)
                    .WithButton("Вперед", "cooking-list-forward", disabled: newPage >= maxPages)
                    .Build();
            });

            return Unit.Value;
        }
    }
}
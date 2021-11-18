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
using Izumi.Services.Game.Ingredient.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Cooking
{
    public record CookingListCommand(SocketSlashCommand Command) : IRequest;

    public class CookingListHandler : IRequestHandler<CookingListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CookingListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CookingListCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var foods = await _mediator.Send(new GetFoodsQuery());

            foods = foods
                .Take(5)
                .ToList();

            var embed = new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются всевозможные рецепты:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Переключай страницы списка рецептов нажимая **Назад** или **Вперед** под этим сообщением." +
                    $"\n\n{emotes.GetEmote("Arrow")} Для приготовления напиши `/приготовить` и укажи количество и название блюда." +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Cooking)))
                .WithFooter("Страница 1");

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

            var component = new ComponentBuilder()
                .WithButton("Назад", "cooking-list-back", disabled: true)
                .WithButton("Вперед", "cooking-list-forward");

            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CheckCookingList));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed, component));
        }
    }
}
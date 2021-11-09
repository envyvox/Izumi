using System;
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
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Ingredient.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Crafting
{
    public record CraftingListCommand(SocketSlashCommand Command) : IRequest;

    public class CraftingListHandler : IRequestHandler<CraftingListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CraftingListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CraftingListCommand request, CancellationToken ct)
        {
            var category = (string) request.Command.Data.Options.First().Value;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            var embed = new EmbedBuilder()
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Crafting)));

            switch (category)
            {
                case "предметов":

                    var craftings = await _mediator.Send(new GetCraftingsQuery());

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "тут отображаются всевозможные рецепты изготовления предметов:" +
                        $"\n\n{emotes.GetEmote("Arrow")} Для изготовления напиши `/изготовить`, выбери предмет, " +
                        "укажи количество и название предмета." +
                        $"\n{StringExtensions.EmptyChar}");

                    foreach (var crafting in craftings)
                    {
                        var costPrice = await _mediator.Send(new GetCraftingCostPriceQuery(crafting.Ingredients));
                        var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                        var ingredientsString = await _mediator.Send(new GetCraftingIngredientsStringQuery(
                            crafting.Ingredients));

                        embed.AddField(
                            $"{emotes.GetEmote("List")} {emotes.GetEmote(crafting.Name)} " +
                            $"{_local.Localize(LocalizationCategoryType.Crafting, crafting.Name)}",
                            $"Ингредиенты: {ingredientsString}" +
                            $"\nСтоимость изготовления: {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}");
                    }

                    break;
                case "алкоголя":

                    var alcohols = await _mediator.Send(new GetAlcoholsQuery());

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "тут отображаются всевозможные рецепты изготовления алкоголя:" +
                        $"\n\n{emotes.GetEmote("Arrow")} Для изготовления напиши `/изготовить`, выбери алкоголь, " +
                        "укажи количество и название алкоголя." +
                        $"\n{StringExtensions.EmptyChar}");

                    foreach (var alcohol in alcohols)
                    {
                        var costPrice = await _mediator.Send(new GetAlcoholCostPriceQuery(alcohol.Ingredients));
                        var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                        var ingredientsString = await _mediator.Send(new GetAlcoholIngredientsStringQuery(
                            alcohol.Ingredients));

                        embed.AddField(
                            $"{emotes.GetEmote("List")} {emotes.GetEmote(alcohol.Name)} " +
                            $"{_local.Localize(LocalizationCategoryType.Alcohol, alcohol.Name)}",
                            $"Ингредиенты: {ingredientsString}" +
                            $"\nСтоимость изготовления: {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}");
                    }

                    break;
                case "напитков":

                    var drinks = await _mediator.Send(new GetDrinksQuery());

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "тут отображаются всевозможные рецепты изготовления напитков:" +
                        $"\n\n{emotes.GetEmote("Arrow")} Для изготовления напиши `/изготовить`, выбери напиток, " +
                        "укажи количество и название напитка." +
                        $"\n{StringExtensions.EmptyChar}");

                    foreach (var drink in drinks)
                    {
                        var costPrice = await _mediator.Send(new GetDrinkCostPriceQuery(drink.Ingredients));
                        var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                        var ingredientsString = await _mediator.Send(new GetDrinkIngredientsStringQuery(
                            drink.Ingredients));

                        embed.AddField(
                            $"{emotes.GetEmote("List")} {emotes.GetEmote(drink.Name)} " +
                            $"{_local.Localize(LocalizationCategoryType.Drink, drink.Name)}",
                            $"Ингредиенты: {ingredientsString}" +
                            $"\nСтоимость изготовления: {emotes.GetEmote(CurrencyType.Ien.ToString())} {craftingPrice} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), craftingPrice)}");
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}
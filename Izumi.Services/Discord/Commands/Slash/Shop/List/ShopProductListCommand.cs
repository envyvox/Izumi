using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Shop.List
{
    public record ShopProductListCommand(SocketSlashCommand Command) : IRequest;

    public class ShopProductListHandler : IRequestHandler<ShopProductListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopProductListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopProductListCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Village);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var products = await _mediator.Send(new GetProductsQuery());

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин продуктов")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются продукты:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/магазин-купить` и выбери продукты из списка вариантов, " +
                    "затем напиши номер желаемого продукта и количество которое ты хочешь приобрести." +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopProduct)));

            var counter = 0;
            foreach (var product in products)
            {
                counter++;

                embed.AddField(
                    $"{emotes.GetEmote("List")} `{product.AutoIncrementedId}` {emotes.GetEmote(product.Name)} " +
                    $"{_local.Localize(LocalizationCategoryType.Product, product.Name)}",
                    $"Стоимость: {emotes.GetEmote(CurrencyType.Ien.ToString())} {product.Price} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), product.Price)}",
                    true);

                if (counter == 2)
                {
                    counter = 0;
                    embed.AddEmptyField(true);
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

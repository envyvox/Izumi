using System.Linq;
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
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Product.Commands;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Shop.Buy
{
    public record ShopProductBuyCommand(SocketSlashCommand Command) : IRequest;

    public class ShopProductBuyHandler : IRequestHandler<ShopProductBuyCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopProductBuyHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopProductBuyCommand request, CancellationToken ct)
        {
            var incId = (long) request.Command.Data.Options.Single(x => x.Name == "номер").Value;
            var amount = request.Command.Data.Options.Any(x => x.Name == "количество")
                ? (uint) (long) request.Command.Data.Options.Single(x => x.Name == "количество").Value
                : 1;

            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Village);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var product = await _mediator.Send(new GetProductByIncIdQuery(incId));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин продуктов");

            var price = product.Price * amount;

            if (userCurrency.Amount < price)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString())} " +
                    $"для приобретения {emotes.GetEmote(product.Name)} {amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Product, product.Name, amount)}.");
            }
            else
            {
                await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, price));
                await _mediator.Send(new AddProductToUserCommand(user.Id, product.Id, amount));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно приобрел {emotes.GetEmote(product.Name)} {amount} " +
                        $"{_local.Localize(LocalizationCategoryType.Product, product.Name, amount)} за " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), price)}.")
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopProduct)));
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

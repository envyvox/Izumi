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
using Izumi.Services.Game.Banner.Commands;
using Izumi.Services.Game.Banner.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Shop.Buy
{
    public record ShopBannerBuyCommand(SocketSlashCommand Command) : IRequest;

    public class ShopBannerBuyHandler : IRequestHandler<ShopBannerBuyCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBannerBuyHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopBannerBuyCommand request, CancellationToken ct)
        {
            var incId = (long) request.Command.Data.Options.Single(x => x.Name == "номер").Value;

            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var banner = await _mediator.Send(new GetDynamicShopBannerByIncIdQuery(incId));
            var hasBanner = await _mediator.Send(new CheckUserHasBannerQuery(user.Id, banner.Id));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин баннеров");

            if (hasBanner)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "");
            }
            else if (userCurrency.Amount < banner.Price)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "");
            }
            else
            {
                await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, banner.Price));
                await _mediator.Send(new AddBannerToUserCommand(user.Id, banner.Id));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно приобрел {banner.Rarity.Localize()} баннер «{banner.Name}» за " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {banner.Price} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), banner.Price)}.")
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopBanner)));
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

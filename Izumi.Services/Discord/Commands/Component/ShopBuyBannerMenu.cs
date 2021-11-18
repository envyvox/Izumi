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
using Izumi.Services.Game.Banner.Commands;
using Izumi.Services.Game.Banner.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Component
{
    public record ShopBuyBannerMenu(SocketMessageComponent Component) : IRequest;

    public class ShopBuyBannerMenuHandler : IRequestHandler<ShopBuyBannerMenu>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuyBannerMenuHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopBuyBannerMenu request, CancellationToken ct)
        {
            var bannerIncId = long.Parse(request.Component.Data.Values.First());

            var user = await _mediator.Send(new GetUserQuery((long) request.Component.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = DiscordRepository.Emotes;
            var banner = await _mediator.Send(new GetDynamicShopBannerByIncIdQuery(bannerIncId));
            var hasBanner = await _mediator.Send(new CheckUserHasBannerQuery(user.Id, banner.Id));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor("Магазин баннеров");

            if (hasBanner)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                    "у тебя уже есть этот баннер, зачем тебе покупать его еще раз?");
            }
            else if (userCurrency.Amount < banner.Price)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                    $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString())} " +
                    $"для приобретения баннера «{banner.Name}»");
            }
            else
            {
                await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, banner.Price));
                await _mediator.Send(new AddBannerToUserCommand(user.Id, banner.Id));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Component.User.Mention}, " +
                        $"ты успешно приобрел {banner.Rarity.Localize()} баннер «{banner.Name}» за " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {banner.Price} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), banner.Price)}.")
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopBanner)));
            }

            await request.Component.FollowupAsync(embed: embed.Build());

            return Unit.Value;
        }
    }
}
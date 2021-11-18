using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Banner.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Shop.List
{
    public record ShopBannerListCommand(SocketSlashCommand Command) : IRequest;

    public class ShopBannerListHandler : IRequestHandler<ShopBannerListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBannerListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopBannerListCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = DiscordRepository.Emotes;
            var banners = await _mediator.Send(new GetDynamicShopBannersQuery());

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин баннеров")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются баннеры:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Для приобретения баннера, **выбери его** из списка под этим сообщением." +
                    $"\n\n{emotes.GetEmote("Arrow")} Это динамический магазин, товары которого обновляются каждый " +
                    "день, не пропускай!" +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopBanner)));

            var selectMenu = new SelectMenuBuilder()
                .WithCustomId("shop-buy-banner")
                .WithPlaceholder("Выбери баннер который хочешь приобрести");

            var counter = 0;
            foreach (var banner in banners)
            {
                counter++;

                embed.AddField(
                    $"{emotes.GetEmote("List")} `{banner.AutoIncrementedId}` {banner.Name}",
                    $"[Нажми сюда чтобы посмотреть]({banner.Url})" +
                    $"\nРедкость: {banner.Rarity.Localize()}" +
                    $"\nСтоимость: {emotes.GetEmote(CurrencyType.Ien.ToString())} {banner.Price} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), banner.Price)}",
                    true);

                if (counter == 2)
                {
                    counter = 0;
                    embed.AddEmptyField(true);
                }

                selectMenu.AddOption(banner.Name.ToLower(), $"{banner.AutoIncrementedId}");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed,
                new ComponentBuilder().WithSelectMenu(selectMenu).Build()));
        }
    }
}
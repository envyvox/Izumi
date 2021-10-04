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

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var banners = await _mediator.Send(new GetDynamicShopBannersQuery());

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин баннеров")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются баннеры:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/магазин-купить` и выбери баннер из списка вариантов, " +
                    "затем напиши номер желаемого баннера." +
                    $"\n\n{emotes.GetEmote("Arrow")} Это динамический магазин, товары которого обновляются каждый " +
                    "день, не пропускай!" +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopBanner)));

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
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

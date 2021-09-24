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
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Seed.Commands;
using Izumi.Services.Game.Seed.Queries;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Shop.Buy
{
    public record ShopSeedBuyCommand(SocketSlashCommand Command) : IRequest;

    public class ShopSeedBuyHandler : IRequestHandler<ShopSeedBuyCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopSeedBuyHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopSeedBuyCommand request, CancellationToken ct)
        {
            var incId = (long) request.Command.Data.Options.Single(x => x.Name == "номер").Value;
            var amount = (uint) (long) request.Command.Data.Options.Single(x => x.Name == "количество").Value;

            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var seed = await _mediator.Send(new GetSeedByIncIdQuery(incId));
            var season = await _mediator.Send(new GetCurrentSeasonQuery());
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

            var price = seed.Price * amount;

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин семян");

            if (seed.Season != season)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"купить можно только семена текущего {emotes.GetEmote(season.EmoteName())} сезона, " +
                    "никаких других на полках магазина не найти.");
            }
            else if (userCurrency.Amount < price)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString())} " +
                    $"для приобретения {emotes.GetEmote(seed.Name)} {amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Seed, seed.Name, amount)}.");
            }
            else
            {
                await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, price));
                await _mediator.Send(new AddSeedToUserCommand(user.Id, seed.Id, amount));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно приобрел {emotes.GetEmote(seed.Name)} {amount} " +
                        $"{_local.Localize(LocalizationCategoryType.Seed, seed.Name, amount)} за " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {price} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), price)}.")
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopSeed)));
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

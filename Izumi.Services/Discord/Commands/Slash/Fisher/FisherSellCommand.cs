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
using Izumi.Services.Game.Fish.Commands;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Fisher
{
    public record FisherSellCommand(SocketSlashCommand Command) : IRequest;

    public class FisherSellCommandHandler : IRequestHandler<FisherSellCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FisherSellCommandHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FisherSellCommand request, CancellationToken ct)
        {
            if (request.Command.Data.Options.First().Name == "все")
                return await SellAllFish(request.Command);

            return await SellFish(request.Command);
        }

        private async Task<Unit> SellFish(SocketSlashCommand command)
        {
            var incId = (long) command.Data.Options.First().Options.Single(x => x.Name == "номер").Value;
            var amount = command.Data.Options.First().Options.Any(x => x.Name == "количество")
                ? (uint) (long) command.Data.Options.First().Options.Single(x => x.Name == "количество").Value
                : 1;

            var user = await _mediator.Send(new GetUserQuery((long) command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Seaport);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var season = await _mediator.Send(new GetCurrentSeasonQuery());
            var fish = await _mediator.Send(new GetFishByIncIdQuery(incId));
            var userFish = await _mediator.Send(new GetUserFishQuery(user.Id, fish.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Дом рыбака")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopFisher)));

            if (!(fish.CatchSeasons.Contains(SeasonType.Any) || fish.CatchSeasons.Contains(season)))
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {command.User.Mention}, " +
                    $"рыбак покупает только рыбу текущего {emotes.GetEmote(season.EmoteName())} сезона " +
                    "или не привязанную к сезону.");
            }
            else if (userFish.Amount < amount)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {command.User.Mention}, " +
                    $"у тебя нет в наличии {emotes.GetEmote(fish.Name)} {amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Fish, fish.Name, amount)}, чтобы продать рыбаку.");
            }
            else
            {
                await _mediator.Send(new RemoveFishFromUserCommand(user.Id, fish.Id, amount));
                await _mediator.Send(new AddCurrencyToUserCommand(user.Id, CurrencyType.Ien, fish.Price * amount));

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {command.User.Mention}, " +
                    $"ты успешно продал рыбаку {emotes.GetEmote(fish.Name)} {amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Fish, fish.Name, amount)} за " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {fish.Price * amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), fish.Price * amount)}.");
            }

            return await _mediator.Send(new RespondEmbedCommand(command, embed));
        }

        private async Task<Unit> SellAllFish(SocketSlashCommand command)
        {
            var user = await _mediator.Send(new GetUserQuery((long) command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Seaport);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var season = await _mediator.Send(new GetCurrentSeasonQuery());
            var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Дом рыбака")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopFisher)));

            uint totalCurrency = 0;
            var soldFishString = string.Empty;

            foreach (var userFish in userFishes
                .Where(x =>
                    x.Fish.CatchSeasons.Contains(SeasonType.Any) ||
                    x.Fish.CatchSeasons.Contains(season)))
            {
                if (userFish.Amount < 1) continue;

                await _mediator.Send(new RemoveFishFromUserCommand(user.Id, userFish.Fish.Id, userFish.Amount));
                await _mediator.Send(new AddCurrencyToUserCommand(
                    user.Id, CurrencyType.Ien, userFish.Fish.Price * userFish.Amount));

                totalCurrency += userFish.Fish.Price * userFish.Amount;
                soldFishString +=
                    $"{emotes.GetEmote(userFish.Fish.Name)} {userFish.Amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Fish, userFish.Fish.Name, userFish.Amount)} за " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {userFish.Fish.Price * userFish.Amount} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), userFish.Fish.Price * userFish.Amount)}\n";
            }

            if (totalCurrency < 1)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {command.User.Mention}, " +
                    $"у тебя нет подходящей по {emotes.GetEmote(season.EmoteName())} сезону рыбы для продажи, " +
                    "не стоит попросту беспокоить рыбака.");
            }
            else
            {
                var descString =
                    soldFishString +
                    $"\n\nИтоговая прибыль {emotes.GetEmote(CurrencyType.Ien.ToString())} {totalCurrency} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), totalCurrency)}";

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {command.User.Mention}, " +
                        "после достаточно быстрых торгов с рыбаком, ты успешно продал ему всю подходящую рыбу:" +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField("Отчетность о продаже",
                        descString.Length > 1024
                            ? "Отчестность была такой длинной, что ты решил сразу взглянуть на самое важное" +
                              $"\n\nИтоговая прибыль {emotes.GetEmote(CurrencyType.Ien.ToString())} {totalCurrency} " +
                              $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), totalCurrency)}"
                            : descString);
            }

            return await _mediator.Send(new RespondEmbedCommand(command, embed));
        }
    }
}

using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Humanizer.Localisation;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Seed.Queries;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.User.Shop.List
{
    public record ShopSeedListCommand(SocketSlashCommand Command) : IRequest;

    public class ShopSeedListHandler : IRequestHandler<ShopSeedListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopSeedListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ShopSeedListCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Capital);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var season = await _mediator.Send(new GetCurrentSeasonQuery());
            var seeds = await _mediator.Send(new GetSeedsBySeasonQuery(season));

            var embed = new EmbedBuilder()
                .WithAuthor("Магазин семян")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются семена текущего сезона:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/магазин-купить` и выбери семена из списка вариантов, " +
                    "затем напиши номер желаемых семян и количество которое ты хочешь приобрести." +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ShopSeed)));

            foreach (var seed in seeds)
            {
                var seedDesc =
                    $"Через {seed.GrowthDays.Days().Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day)} вырастет " +
                    $"{emotes.GetEmote(seed.Crop.Name)} {_local.Localize(LocalizationCategoryType.Crop, seed.Crop.Name)} " +
                    $"стоимостью {emotes.GetEmote(CurrencyType.Ien.ToString())} {seed.Crop.Price} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), seed.Crop.Price)}";

                if (seed.IsMultiply)
                    seedDesc +=
                        $"\n{emotes.GetEmote("Arrow")} *Растет несколько шт. с одного семени*";

                if (seed.ReGrowthDays > 0)
                    seedDesc +=
                        $"\n{emotes.GetEmote("Arrow")} *После первого сбора будет давать урожай каждые " +
                        $"{seed.ReGrowthDays.Days().Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day)}*";

                embed.AddField(
                    $"{emotes.GetEmote("List")} `{seed.AutoIncrementedId}` {emotes.GetEmote(seed.Name)} " +
                    $"{_local.Localize(LocalizationCategoryType.Seed, seed.Name)} стоимостью " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {seed.Price} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), seed.Price)}",
                    seedDesc + $"\n{StringExtensions.EmptyChar}");
            }

            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CheckSeedShop));
            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

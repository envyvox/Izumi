using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Game.Field.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.User.Field
{
    public record FieldInfoCommand(SocketSlashCommand Command) : IRequest;

    public class FieldInfoHandler : IRequestHandler<FieldInfoCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldInfoHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FieldInfoCommand request, CancellationToken cancellationToken)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userFields = await _mediator.Send(new GetUserFieldsQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Участок")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Field)));

            if (userFields.Any())
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"тут отображается твой {emotes.GetEmote("HarvestField")} участок земли:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Для начала необходимо `/участок посадить [номер клетки] [название семян]` семена на твои клетки земли." +
                    $"\n\n{emotes.GetEmote("Arrow")} Семена необходимо `/участок полить` каждый день, кроме {emotes.GetEmote(WeatherType.Rain.EmoteName())} дождливых." +
                    $"\n\n{emotes.GetEmote("Arrow")} После того как семена созреют, ты можешь `/участок собрать [номер клетки]` готовый урожай." +
                    $"\n\n{emotes.GetEmote("Arrow")} Если ты передумал выращивать семена на этой клетке или хочешь их заменить - `/участок выкопать [номер клетки]` и они будут удалены." +
                    $"\n{StringExtensions.EmptyChar}");

                foreach (var field in userFields)
                {
                    string fieldName;
                    string fieldDesc;

                    switch (field.State)
                    {
                        case FieldStateType.Empty:

                            fieldName = "Клетка земли пустая";
                            fieldDesc = "Посади на нее семена чтобы начать выращивать урожай";

                            break;
                        case FieldStateType.Planted:
                        {
                            var growthDays = field.InReGrowth
                                ? (field.Seed.ReGrowthDays - field.Progress)
                                .Days().Humanize(1, new CultureInfo("ru-RU"))
                                : (field.Seed.GrowthDays - field.Progress)
                                .Days().Humanize(1, new CultureInfo("ru-RU"));

                            fieldName =
                                $"{emotes.GetEmote(field.Seed.Name)} " +
                                $"{_local.Localize(LocalizationCategoryType.Seed, field.Seed.Name)}, " +
                                $"еще {growthDays} роста";

                            fieldDesc = "Не забудь сегодня полить";

                            break;
                        }

                        case FieldStateType.Watered:
                        {
                            var growthDays = field.InReGrowth
                                ? (field.Seed.ReGrowthDays - field.Progress)
                                .Days().Humanize(1, new CultureInfo("ru-RU"))
                                : (field.Seed.GrowthDays - field.Progress)
                                .Days().Humanize(1, new CultureInfo("ru-RU"));

                            fieldName =
                                $"{emotes.GetEmote(field.Seed.Name)} " +
                                $"{_local.Localize(LocalizationCategoryType.Seed, field.Seed.Name)}, " +
                                $"еще {growthDays} роста";

                            fieldDesc = "Поливать сегодня уже не нужно";

                            break;
                        }

                        case FieldStateType.Completed:

                            fieldName =
                                $"{emotes.GetEmote(field.Seed.Crop.Name)} " +
                                $"{_local.Localize(LocalizationCategoryType.Crop, field.Seed.Crop.Name)}, можно собирать";

                            fieldDesc = field.Seed.ReGrowthDays > 0
                                ? "После сбора будет давать урожай каждые " +
                                  $"{field.Seed.ReGrowthDays.Days().Humanize(1, new CultureInfo("ru-RU"))}"
                                : "Не забудь посадить что-то на освободившееся место";

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    embed.AddField($"{emotes.GetEmote("List")} `{field.Number}` {fieldName}", fieldDesc);
                }
            }
            else
            {
                var fieldPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.EconomyFieldPrice));

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя еще нет {emotes.GetEmote("HarvestField")} участка земли, напиши `/участок купить` чтобы приобрести его за " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {fieldPrice} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), fieldPrice)}.");
            }

            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CheckField));
            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

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
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Collection.Commands;
using Izumi.Services.Game.Crop.Commands;
using Izumi.Services.Game.Field.Commands;
using Izumi.Services.Game.Field.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Field
{
    public record FieldCollectCommand(SocketSlashCommand Command) : IRequest;

    public class FieldCollectHandler : IRequestHandler<FieldCollectCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly Random _random = new();

        public FieldCollectHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FieldCollectCommand request, CancellationToken cancellationToken)
        {
            var number = (uint) (long) request.Command.Data.Options.Single(x => x.Name == "номер").Value;

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userField = await _mediator.Send(new GetUserFieldQuery(user.Id, number));

            var embed = new EmbedBuilder()
                .WithAuthor("Сбор урожая")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Field)));

            switch (userField.State)
            {
                case FieldStateType.Empty:

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "на этой ячейке ничего не растет, сперва посади семена.");

                    break;
                case FieldStateType.Planted:
                case FieldStateType.Watered:

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "семена на этой клетке еще не созрели для сбора, поливай их каждый день и ожидай своего урожая.");

                    break;
                case FieldStateType.Completed:

                    var amount = userField.Seed.IsMultiply
                        ? (uint) _random.Next(2, 4)
                        : 1;
                    var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.EnergyCostFieldCollect));

                    if (userField.Seed.ReGrowthDays > 0)
                    {
                        await _mediator.Send(new StartReGrowthOnUserFieldCommand(user.Id, number));

                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"ты успешно собрал {emotes.GetEmote(userField.Seed.Crop.Name)} {amount} " +
                            $"{_local.Localize(LocalizationCategoryType.Crop, userField.Seed.Crop.Name, amount)}. " +
                            $"Новый урожай через {userField.Seed.ReGrowthDays.Days().Humanize(1, new CultureInfo("ru-RU"))} дней.");
                    }
                    else
                    {
                        await _mediator.Send(new ResetUserFieldCommand(user.Id, number));

                        embed.WithDescription(
                            $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                            $"ты успешно собрал {emotes.GetEmote(userField.Seed.Crop.Name)} {amount} " +
                            $"{_local.Localize(LocalizationCategoryType.Crop, userField.Seed.Crop.Name, amount)}, " +
                            "ячейка земли теперь свободна.");
                    }

                    await _mediator.Send(new AddCropToUserCommand(user.Id, userField.Seed.Crop.Id, amount));
                    await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost));
                    await _mediator.Send(new AddCollectionToUserCommand(
                        user.Id, CollectionType.Crop, userField.Seed.Crop.Id));
                    await _mediator.Send(new AddStatisticToUserCommand(user.Id, StatisticType.CropHarvested));
                    await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                    {
                        AchievementType.Collect50Crop,
                        AchievementType.Collect300Crop,
                        AchievementType.CompleteCollectionCrop
                    }));

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

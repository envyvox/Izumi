using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Collection.Commands;
using Izumi.Services.Game.Gathering.Commands;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreCastle
{
    public class CompleteExploreCastleJob : ICompleteExploreCastleJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly ILogger<CompleteExploreCastleJob> _logger;

        public CompleteExploreCastleJob(
            IMediator mediator,
            ILocalizationService local,
            ILogger<CompleteExploreCastleJob> logger)
        {
            _mediator = mediator;
            _local = local;
            _logger = logger;
        }

        public async Task Execute(long userId)
        {
            _logger.LogInformation(
                "Complete explore castle job executed for user {UserId}",
                userId);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(
                LocationType.ExploreCastle));

            await _mediator.Send(new UpdateUserLocationCommand(userId, LocationType.Castle));
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireJobType.Explore));
            await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.ExploreCastle));

            var gatheringString = string.Empty;
            uint itemsCount = 0;

            foreach (var gathering in gatherings)
            {
                var chance = await _mediator.Send(new GetGatheringPropertyValueQuery(
                    gathering.Id, GatheringPropertyType.GatheringChance));
                var doubleChance = await _mediator.Send(new GetGatheringPropertyValueQuery(
                    gathering.Id, GatheringPropertyType.GatheringDoubleChance));
                var amount = await _mediator.Send(new GetGatheringPropertyValueQuery(
                    gathering.Id, GatheringPropertyType.GatheringAmount));
                var successAmount = await _mediator.Send(new GetSuccessAmountQuery(chance, doubleChance, amount));

                if (successAmount < 1) continue;

                await _mediator.Send(new AddGatheringToUserCommand(userId, gathering.Id, successAmount));
                await _mediator.Send(new AddCollectionToUserCommand(userId, CollectionType.Gathering, gathering.Id));

                gatheringString +=
                    $"{emotes.GetEmote(gathering.Name)} {successAmount} " +
                    $"{_local.Localize(LocalizationCategoryType.Gathering, gathering.Name, successAmount)}, ";
                itemsCount += successAmount;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(LocationType.ExploreCastle.Localize())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.ExploreCastle)));

            if (itemsCount > 0)
            {
                await _mediator.Send(new AddStatisticToUserCommand(userId, StatisticType.Gathering, itemsCount));

                embed
                    .WithDescription("Ты вернулся с исследования шахты и был приятно удивлен тяжестью своей корзины.")
                    .AddField("Полученные ресурсы", gatheringString.RemoveFromEnd(2));
            }
            else
            {
                embed.WithDescription(
                    "Ты знаешь это ощущение, когда приходишь вглубь шахты и не помнишь зачем пришел? " +
                    "Именно такое ощущение пришло к тебе когда ты добрался до места назначения, и только вернувшись " +
                    "обратно ты вспомнил что ходил то за ресурсами.");
            }

            await _mediator.Send(new CheckAchievementsInUserCommand(userId, new[]
            {
                AchievementType.FirstGatheringResource,
                AchievementType.Gather40Resources,
                AchievementType.Gather250Resources
            }));
            await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.CompleteExploreCastle));
            await _mediator.Send(new SendEmbedToUserCommand((ulong) userId, embed));
        }
    }
}

using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Contract.Commands;
using Izumi.Services.Game.Contract.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Reputation.Commands;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteContract
{
    public class CompleteContractJob : ICompleteContractJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly ILogger<CompleteContractJob> _logger;

        public CompleteContractJob(
            IMediator mediator,
            ILocalizationService local,
            ILogger<CompleteContractJob> logger)
        {
            _mediator = mediator;
            _local = local;
            _logger = logger;
        }

        public async Task Execute(long userId, long contractIncId)
        {
            _logger.LogInformation(
                "Complete contract job executed for user {UserId} with contract inc id {IncId}",
                userId, contractIncId);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery(userId));
            var contract = await _mediator.Send(new GetContractQuery(contractIncId));

            await _mediator.Send(new UpdateUserLocationCommand(user.Id, contract.Location));
            await _mediator.Send(new DeleteUserMovementCommand(user.Id));
            await _mediator.Send(new DeleteUserContractCommand(user.Id));
            await _mediator.Send(new DeleteUserHangfireJobCommand(user.Id, HangfireJobType.Contract));
            await _mediator.Send(new AddCurrencyToUserCommand(user.Id, CurrencyType.Ien, contract.CurrencyReward));
            await _mediator.Send(new AddReputationToUserCommand(
                user.Id, contract.Location.Reputation(), contract.ReputationReward));
            await _mediator.Send(new AddStatisticToUserCommand(user.Id, StatisticType.Contracts));
            await _mediator.Send(new CheckAchievementInUserCommand(user.Id, AchievementType.FirstContract));

            var embed = new EmbedBuilder()
                .WithAuthor(contract.Name)
                .WithDescription(
                    "С возвращением, жители говорят что ты отлично потрудился и заслуживаешь положенной награды, " +
                    "она уже ожидает тебя в твоем инвентаре. Про репутацию я тоже не забыла, не переживай!" +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Награда",
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {contract.CurrencyReward} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), contract.CurrencyReward)} " +
                    $"и {emotes.GetEmote(contract.Location.Reputation().Emote(int.MaxValue))} {contract.ReputationReward} " +
                    $"репутации в **{contract.Location.Localize(true)}**")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Contracts)));

            await _mediator.Send(new SendEmbedToUserCommand((ulong) user.Id, embed));
        }
    }
}

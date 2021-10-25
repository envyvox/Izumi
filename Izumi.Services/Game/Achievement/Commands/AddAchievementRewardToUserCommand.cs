using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Title.Commands;
using MediatR;

namespace Izumi.Services.Game.Achievement.Commands
{
    public record AddAchievementRewardToUserCommand(long UserId, AchievementType Type) : IRequest;

    public class AddAchievementRewardToUserHandler : IRequestHandler<AddAchievementRewardToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public AddAchievementRewardToUserHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(AddAchievementRewardToUserCommand request, CancellationToken cancellationToken)
        {
            var emotes = DiscordRepository.Emotes;
            var achievement = await _mediator.Send(new GetAchievementQuery(request.Type));

            switch (achievement.RewardType)
            {
                case AchievementRewardType.Ien:

                    await _mediator.Send(new AddCurrencyToUserCommand(
                        request.UserId, CurrencyType.Ien, achievement.RewardNumber));

                    break;
                case AchievementRewardType.Title:

                    await _mediator.Send(new AddTitleToUserCommand(
                        request.UserId, (TitleType) achievement.RewardNumber));

                    break;
                case AchievementRewardType.Pearl:

                    await _mediator.Send(new AddCurrencyToUserCommand(
                        request.UserId, CurrencyType.Pearl, achievement.RewardNumber));

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var embed = new EmbedBuilder()
                .WithAuthor("Достижения")
                .WithDescription(
                    $"Поздравляю с выполнением достижения {emotes.GetEmote("Achievement")} **{achievement.Name}** " +
                    $"из категории **{achievement.Category.Localize()}** и получаете " +
                    achievement.RewardType switch
                    {
                        AchievementRewardType.Ien =>
                            $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {achievement.RewardNumber} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), achievement.RewardNumber)}.",
                        AchievementRewardType.Title =>
                            $"титул {emotes.GetEmote(((TitleType) achievement.RewardNumber).EmoteName())} " +
                            $"{((TitleType) achievement.RewardNumber).Localize()}.",
                        AchievementRewardType.Pearl =>
                            $"{emotes.GetEmote(CurrencyType.Pearl.ToString())} {achievement.RewardNumber} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), achievement.RewardNumber)}.",
                        _ => throw new ArgumentOutOfRangeException()
                    });

            return await _mediator.Send(new SendEmbedToUserCommand((ulong) request.UserId, embed));
        }
    }
}

using System;
using System.Globalization;
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
using Izumi.Services.Game.Achievement.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Info
{
    public record AchievementsCommand(SocketSlashCommand Command) : IRequest;

    public class AchievementsHandler : IRequestHandler<AchievementsCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public AchievementsHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(AchievementsCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var category = (AchievementCategoryType) (long) request.Command.Data.Options.First().Value;
            var achievements = await _mediator.Send(new GetAchievementsInCategoryQuery(category));
            var userAchievements = await _mediator.Send(new GetUserAchievementsInCategoryQuery(
                user.Id, category));

            var embed = new EmbedBuilder()
                .WithAuthor("Достижения")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"тут отображаются твои достижения в категории **{category.Localize()}**:" +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Achievements)));

            foreach (var achievement in achievements)
            {
                var userAchievement = userAchievements.SingleOrDefault(x => x.Achievement.Type == achievement.Type);
                var exist = userAchievement is not null;

                embed.AddField($"{emotes.GetEmote("Achievement" + (exist ? "" : "BW"))} {achievement.Name}",
                    "Награда: " +
                    achievement.RewardType switch
                    {
                        AchievementRewardType.Ien =>
                            $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {achievement.RewardNumber} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), achievement.RewardNumber)}",
                        AchievementRewardType.Title =>
                            $"титул {emotes.GetEmote(((TitleType) achievement.RewardNumber).EmoteName())} " +
                            $"{((TitleType) achievement.RewardNumber).Localize()}",
                        AchievementRewardType.Pearl =>
                            $"{emotes.GetEmote(CurrencyType.Pearl.ToString())} {achievement.RewardNumber} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), achievement.RewardNumber)}",
                        _ => throw new ArgumentOutOfRangeException()
                    } + (exist
                        ? $"\nВыполнено в {userAchievement.CreatedAt.ToString("HH:MM, dd MMMM yyyy", new CultureInfo("ru-RU"))}"
                        : ""));
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

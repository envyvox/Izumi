using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.Cooldown.Commands;
using Izumi.Services.Game.Cooldown.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Queries;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash
{
    public record DailyRewardCommand(SocketSlashCommand Command) : IRequest;

    public class DailyRewardHandler : IRequestHandler<DailyRewardCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public DailyRewardHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(DailyRewardCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userCooldown = await _mediator.Send(new GetUserCooldownQuery(user.Id, CooldownType.DailyReward));
            var userTutorial = await _mediator.Send(new GetUserTutorialStepQuery(user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Ежедневная награда");

            if (userTutorial is not TutorialStepType.Completed)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ежедневная награда доступна только после прохождения `/обучение`.");
            }
            else if (userCooldown.Expiration > DateTimeOffset.UtcNow)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты уже получал ежедневную награду сегодня, возвращайся через " +
                    $"**{(userCooldown.Expiration - DateTimeOffset.UtcNow).Humanize(2, new CultureInfo("ru-RU"))}**.");
            }
            else
            {
                var dailyReward = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.EconomyDailyReward));

                await _mediator.Send(new AddCurrencyToUserCommand(user.Id, CurrencyType.Ien, dailyReward));
                await _mediator.Send(new CreateUserCooldownCommand(
                    user.Id, CooldownType.DailyReward, TimeSpan.FromHours(22)));

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"ты получил ежедневную награду ввиде {emotes.GetEmote(CurrencyType.Ien.ToString())} {dailyReward} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), dailyReward)}.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

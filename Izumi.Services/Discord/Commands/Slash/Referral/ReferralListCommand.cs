using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Referral.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Referral
{
    public record ReferralListCommand(SocketSlashCommand Command) : IRequest;

    public class ReferralListHandler : IRequestHandler<ReferralListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ReferralListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ReferralListCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userReferrals = await _mediator.Send(new GetUserReferralsQuery(user.Id));
            var hasReferrer = await _mediator.Send(new CheckUserHasReferrerQuery(user.Id));

            string referrerString;
            if (hasReferrer)
            {
                var referrer = await _mediator.Send(new GetUserReferrerQuery(user.Id));
                var socketReferrer = await _mediator.Send(new GetSocketGuildUserQuery((ulong) referrer.Id));

                referrerString =
                    $"Ты указал {emotes.GetEmote(referrer.Title.EmoteName())} {referrer.Title.Localize()} {socketReferrer.Mention} " +
                    $"как пригласившего тебя пользователя и получил {emotes.GetEmote(BoxType.Capital.EmoteName())} " +
                    $"{_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString())}.";
            }
            else
            {
                referrerString =
                    "Ты не указал пользователя который тебя пригласил." +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/пригласил @Пользователь` и получи " +
                    $"{emotes.GetEmote(BoxType.Capital.EmoteName())} " +
                    $"{_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString())}.";
            }

            var referralString = string.Empty;

            foreach (var userReferral in userReferrals)
            {
                var socketUserReferral = await _mediator.Send(new GetSocketGuildUserQuery((ulong) userReferral.Id));

                referralString +=
                    $"{emotes.GetEmote("List")} {emotes.GetEmote(userReferral.Title.EmoteName())} {userReferral.Title.Localize()} {socketUserReferral.Mention}\n";
            }

            var embed = new EmbedBuilder()
                .WithAuthor("Реферальная система")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображается информация о твоем участии в реферальной системе:" +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Твой реферер",
                    referrerString + $"\n{StringExtensions.EmptyChar}")
                .AddField("Награды реферальной системы",
                    $"{emotes.GetEmote(userReferrals.Count >= 2 ? "Checkmark" : "List")} За `1`, `2` приглашенных ты получишь {emotes.GetEmote(BoxType.Capital.EmoteName())} {_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString())}.\n" +
                    $"{emotes.GetEmote(userReferrals.Count >= 4 ? "Checkmark" : "List")} За `3`, `4` приглашенных ты получишь {emotes.GetEmote(BoxType.Capital.EmoteName())} 2 {_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString(), 2)}.\n" +
                    $"{emotes.GetEmote(userReferrals.Count >= 5 ? "Checkmark" : "List")} За `5` приглашенных ты получишь {emotes.GetEmote(BoxType.Capital.EmoteName())} 3 {_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString(), 3)}.\n" +
                    $"{emotes.GetEmote(userReferrals.Count >= 9 ? "Checkmark" : "List")} За `6`, `7`, `8`, `9` пользователя ты получишь {emotes.GetEmote(CurrencyType.Pearl.ToString())} 10 {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), 10)}.\n" +
                    $"{emotes.GetEmote(userReferrals.Count >= 10 ? "Checkmark" : "List")} За `10` приглашенных ты получишь {emotes.GetEmote(CurrencyType.Pearl.ToString())} 10 {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), 10)} и титул {emotes.GetEmote(TitleType.Yatagarasu.EmoteName())} {TitleType.Yatagarasu.Localize()}.\n" +
                    $"{emotes.GetEmote("List")} За каждого последующего ты будешь получать {emotes.GetEmote(CurrencyType.Pearl.ToString())} 15 {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), 15)}." +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Приглашенные пользователи",
                    referralString.Length > 0
                        ? referralString.Length > 1024
                            ? "У тебя так много приглашенных пользователей, что мне трудно назвать их всех! " +
                              $"Но их точно **{userReferrals.Count}**"
                            : referrerString
                        : "Ты еще не пригласил ни одного пользователя.\nПриглашай друзей и получайте " +
                          $"{emotes.GetEmote(BoxType.Capital.EmoteName())} {emotes.GetEmote(CurrencyType.Pearl.ToString())} " +
                          "бонусы реферальной системы вместе.");

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

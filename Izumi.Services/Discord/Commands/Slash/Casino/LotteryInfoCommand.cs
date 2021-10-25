using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Effect.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Casino
{
    public record LotteryInfoCommand(SocketSlashCommand Command) : IRequest;

    public class LotteryInfoHandler : IRequestHandler<LotteryInfoCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public LotteryInfoHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(LotteryInfoCommand request, CancellationToken ct)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var lotteryUsers = await _mediator.Send(new GetUsersWithEffectQuery(EffectType.Lottery));

            var lotteryPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.CasinoLotteryPrice));
            var lotteryReward = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.CasinoLotteryReward));
            var lotteryDeliveryPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.CasinoLotteryDeliveryPrice));
            var lotteryRequireUsers = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.CasinoLotteryRequiredUsersCount));

            var lotteryUsersString = string.Empty;

            foreach (var lotteryUser in lotteryUsers)
            {
                var socketUser = await _mediator.Send(new GetSocketGuildUserQuery((ulong) lotteryUser.Id));

                lotteryUsersString +=
                    $"{emotes.GetEmote(lotteryUser.Title.EmoteName())} {lotteryUser.Title.Localize()} {socketUser.Mention}\n";
            }

            var embed = new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображается информация о проведении лотерии:" +
                    $"\n{StringExtensions.EmptyChar}")
                .AddField("Как принять участие",
                    $"Напиши `/лотерея купить` чтобы приобрести {emotes.GetEmote("LotteryTicket")} лотерейный билет за " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {lotteryPrice} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), lotteryPrice)} " +
                    $"и ожидай, когда наберется **{lotteryRequireUsers} участников**." +
                    "\n\nЗатем будет выбран **один победитель** который получит " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {lotteryReward} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), lotteryReward)}." +
                    $"\n{emotes.GetEmote("Arrow")} Победитель выбирается случайным образом среди всех участков.")
                .AddField("Служба доставки",
                    "Напиши `/лотерея подарить` и выбери необходимого пользователя чтобы отправить ему " +
                    $"{emotes.GetEmote("LotteryTicket")} лотерейный билет в **подарок**. Курьер доставит его в любую точку мира!" +
                    $"\n{emotes.GetEmote("Arrow")} При оплате доставки, необходимо будет доплатить " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {lotteryDeliveryPrice} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), lotteryDeliveryPrice)} " +
                    "за услуги курьерской службы.")
                .AddField("Текущие участники",
                    lotteryUsersString.Length > 0
                        ? lotteryUsersString
                        : $"На данный момент нет пользователей с {emotes.GetEmote("LotteryTicket")} лотерейным билетом.");

            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CheckLottery));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

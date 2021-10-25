using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash
{
    public record SendIenCommand(SocketSlashCommand Command) : IRequest;

    public class SendIenCommandHandler : IRequestHandler<SendIenCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public SendIenCommandHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(SendIenCommand request, CancellationToken ct)
        {
            var socketTarget = (SocketGuildUser) request.Command.Data.Options
                .Single(x => x.Name == "пользователь").Value;
            var amount = (uint) (long) request.Command.Data.Options
                .Single(x => x.Name == "количество").Value;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var target = await _mediator.Send(new GetUserQuery((long) socketTarget.Id));

            var embed = new EmbedBuilder();

            if (socketTarget.IsBot)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"ты не можешь отправить {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 2)} боту.");
            }
            else if (user.Id == target.Id)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"зачем тебе передавать самому себе {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 2)}?");
            }
            else
            {
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

                if (amount < 10)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"количество должно быть больше или равно {emotes.GetEmote(CurrencyType.Ien.ToString())} 10 " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 10)} для отправки.");
                }
                else if (userCurrency.Amount < amount)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"у тебя нет столько {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), 2)} для отправки.");
                }
                else
                {
                    var taxPercent = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.SendIenTaxPercent));
                    var amountAfterTax = (uint) (user.IsPremium
                        ? amount
                        : amount - amount / 100.0 * taxPercent);

                    await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, amount));
                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, CurrencyType.Ien, amountAfterTax));

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно отправил {emotes.GetEmote(target.Title.EmoteName())} {target.Title.Localize()} " +
                        $"{socketTarget.Mention} {emotes.GetEmote(CurrencyType.Ien.ToString())} {amountAfterTax} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), amountAfterTax)}, " +
                        $"коммиссия составила {emotes.GetEmote(CurrencyType.Ien.ToString())} {amount - amountAfterTax} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), amount - amountAfterTax)}.");

                    var embedNotify = new EmbedBuilder()
                        .WithDescription(
                            $"Ты получил {emotes.GetEmote(CurrencyType.Ien.ToString())} {amountAfterTax} " +
                            $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), amountAfterTax)} " +
                            $"от {emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}.");

                    await _mediator.Send(new SendEmbedToUserCommand(socketTarget.Id, embedNotify));
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

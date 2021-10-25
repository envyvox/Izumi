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
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Cooldown.Commands;
using Izumi.Services.Game.Cooldown.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash
{
    public record RenameCommand(SocketSlashCommand Command) : IRequest;

    public class RenameCommandHandler : IRequestHandler<RenameCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public RenameCommandHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(RenameCommand request, CancellationToken ct)
        {
            var nickname = (string) request.Command.Data.Options.First().Value;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var userCooldown = await _mediator.Send(new GetUserCooldownQuery(user.Id, CooldownType.Rename));

            var embed = new EmbedBuilder();

            if (userCooldown.Expiration > DateTimeOffset.UtcNow)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "ты недавно изменял свой никнейм на сервере, необходимо подождать еще " +
                    $"**{(DateTimeOffset.UtcNow - userCooldown.Expiration).Humanize(2, new CultureInfo("ru-RU"))}**.");
            }
            else
            {
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));
                var renamePrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.RenamePrice));

                if (userCurrency.Amount < renamePrice)
                {
                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"для изменения никнейма необходимо иметь {emotes.GetEmote(CurrencyType.Ien.ToString())} {renamePrice} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), renamePrice)}, " +
                        "которых у тебя нет.");
                }
                else
                {
                    var renameCooldown = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldPropertyType.RenameCooldownDays));

                    await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, renamePrice));
                    await _mediator.Send(new CreateUserCooldownCommand(
                        user.Id, CooldownType.Rename, TimeSpan.FromDays(renameCooldown)));
                    await _mediator.Send(new RenameGuildUserCommand(request.Command.User.Id, nickname));

                    embed.WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"твой никнейм на сервере успешно изменен на **{nickname}** за " +
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {renamePrice} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), renamePrice)}.");
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

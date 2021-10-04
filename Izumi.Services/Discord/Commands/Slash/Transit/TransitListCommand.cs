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
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Transit.Queries;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Transit
{
    public record TransitListCommand(SocketSlashCommand Command) : IRequest;

    public class TransitListHandler : IRequestHandler<TransitListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public TransitListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(TransitListCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var transits = await _mediator.Send(new GetTransitsFromLocationQuery(user.Location));

            var embed = new EmbedBuilder()
                .WithAuthor("Отправления")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображаются доступные отправления из твоей локации:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/отправиться` и выбери локацию из списка вариантов." +
                    $"\n{StringExtensions.EmptyChar}")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.TransitList)))
                .WithFooter("* Длительность указана с учетом твоей текущей энергии.");

            var counter = 0;
            foreach (var transit in transits)
            {
                counter++;

                var transitTime = await _mediator.Send(new GetActionTimeQuery(transit.Duration, user.Energy));

                embed.AddField(transit.Destination.Localize(),
                    $"Стоимость: {emotes.GetEmote(CurrencyType.Ien.ToString())} {transit.Price} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), transit.Price)}" +
                    $"\nДлительность: {transitTime.Humanize(2, new CultureInfo("ru-RU"))}", true);

                if (counter == 2)
                {
                    counter = 0;
                    embed.AddEmptyField(true);
                }
            }

            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CheckTransits));
            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

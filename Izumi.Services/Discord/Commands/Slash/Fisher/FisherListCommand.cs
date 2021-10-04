using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Fisher
{
    public record FisherListCommand(SocketSlashCommand Command) : IRequest;

    public class FisherListCommandHandler : IRequestHandler<FisherListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FisherListCommandHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FisherListCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Seaport);

            var emotes = await _mediator.Send(new GetEmotesQuery());
            var season = await _mediator.Send(new GetCurrentSeasonQuery());
            var fishes = await _mediator.Send(new GetFishesBySeasonQuery(season));

            var embed = new EmbedBuilder()
                .WithAuthor("Дом рыбака")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "тут отображается рыба, которую рыбак согласен купить у тебя:" +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/рыбак-продать` чтобы продать определенную рыбу или всю подходяющую." +
                    $"\n{StringExtensions.EmptyChar}");

            foreach (var fish in fishes)
            {
                embed.AddField(
                    $"`{fish.AutoIncrementedId}` {emotes.GetEmote(fish.Name)} " +
                    $"{_local.Localize(LocalizationCategoryType.Fish, fish.Name)}",
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {fish.Price} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), fish.Price)} за 1шт." +
                    $"\n{StringExtensions.EmptyChar}",
                    true);
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

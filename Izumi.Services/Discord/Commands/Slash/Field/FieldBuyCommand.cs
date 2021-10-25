using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Building.Commands;
using Izumi.Services.Game.Building.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Field.Commands;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Field
{
    public record FieldBuyCommand(SocketSlashCommand Command) : IRequest;

    public class FieldBuyHandler : IRequestHandler<FieldBuyCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldBuyHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FieldBuyCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Village);

            var emotes = DiscordRepository.Emotes;
            var fieldPrice = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EconomyFieldPrice));
            var building = await _mediator.Send(new GetBuildingQuery(BuildingType.HarvestField));
            var hasBuilding = await _mediator.Send(new CheckUserHasBuildingQuery(user.Id, building.Type));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, CurrencyType.Ien));

            var embed = new EmbedBuilder()
                .WithAuthor("Приобретение участка")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Field)));

            if (hasBuilding)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя уже есть {emotes.GetEmote(building.Type.ToString())} {building.Name}.");
            }
            else if (userCurrency.Amount < fieldPrice)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя недостаточно {emotes.GetEmote(CurrencyType.Ien.ToString())} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString())} " +
                    $"для приобретения {emotes.GetEmote(building.Type.ToString())} {building.Name}.");
            }
            else
            {
                await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, CurrencyType.Ien, fieldPrice));
                await _mediator.Send(new AddBuildingToUserCommand(user.Id, building.Type));
                await _mediator.Send(new CreateUserFieldsCommand(user.Id, new uint[] { 1, 2, 3, 4, 5 }));

                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"ты успешно приобрел {emotes.GetEmote(building.Type.ToString())} {building.Name} за " +
                    $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {fieldPrice} " +
                    $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), fieldPrice)}.");
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

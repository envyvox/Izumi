using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using Izumi.Services.Game.Field.Commands;
using Izumi.Services.Game.Field.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Localization.Queries;
using Izumi.Services.Game.Seed.Commands;
using Izumi.Services.Game.Seed.Queries;
using Izumi.Services.Game.Statistic.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Game.World.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.Field
{
    public record FieldPlantCommand(SocketSlashCommand Command) : IRequest;

    public class FieldPlantHandler : IRequestHandler<FieldPlantCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldPlantHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(FieldPlantCommand request, CancellationToken cancellationToken)
        {
            var number = (uint) (long) request.Command.Data
                .Options.First()
                .Options.Single(x => x.Name == "номер").Value;
            var seedName = (string) request.Command.Data
                .Options.First()
                .Options.Single(x => x.Name == "название").Value;

            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            user.Location.CheckRequiredLocation(LocationType.Village);

            var localization = await _mediator.Send(new GetLocalizationByLocalizedNameQuery(
                LocalizationCategoryType.Seed, seedName));
            var emotes = DiscordRepository.Emotes;
            var seed = await _mediator.Send(new GetSeedByNameQuery(localization.Name));
            var userSeed = await _mediator.Send(new GetUserSeedQuery(user.Id, seed.Id));
            var userField = await _mediator.Send(new GetUserFieldQuery(user.Id, number));

            var embed = new EmbedBuilder()
                .WithAuthor("Посадка семян на участок")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Field)));

            if (userField.State != FieldStateType.Empty)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"посадить {emotes.GetEmote(seed.Name)} {_local.Localize(LocalizationCategoryType.Seed, seed.Name)} " +
                    $"можно только на пустую клетку {emotes.GetEmote(BuildingType.HarvestField.ToString())} участка.");
            }
            else if (userSeed.Amount < 1)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"у тебя нет в наличии {emotes.GetEmote(seed.Name)} " +
                    $"{_local.Localize(LocalizationCategoryType.Seed, seed.Name)}.");
            }
            else
            {
                var energyCost = await _mediator.Send(new GetWorldPropertyValueQuery(
                    WorldPropertyType.EnergyCostFieldPlant));

                await _mediator.Send(new RemoveSeedFromUserCommand(user.Id, seed.Id, 1));
                await _mediator.Send(new PlantUserFieldCommand(user.Id, number, seed.Id));
                await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, energyCost));
                await _mediator.Send(new AddStatisticToUserCommand(user.Id, StatisticType.SeedPlanted));
                await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
                {
                    AchievementType.FirstPlant,
                    AchievementType.Plant25Seed,
                    AchievementType.Plant150Seed
                }));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        $"ты успешно посадил {emotes.GetEmote(seed.Name)} " +
                        $"{_local.Localize(LocalizationCategoryType.Seed, seed.Name)} на клетку своего " +
                        $"{emotes.GetEmote(BuildingType.HarvestField.ToString())} участка." +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField("Расход энергии",
                        $"{emotes.GetEmote("Energy")} {energyCost} " +
                        $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", energyCost)}",
                        true);
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

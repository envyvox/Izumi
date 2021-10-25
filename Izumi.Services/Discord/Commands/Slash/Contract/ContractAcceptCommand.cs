using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hangfire;
using Humanizer;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Contract.Commands;
using Izumi.Services.Game.Contract.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteContract;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Contract
{
    public record ContractAcceptCommand(SocketSlashCommand Command) : IRequest;

    public class ContractAcceptHandler : IRequestHandler<ContractAcceptCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ContractAcceptHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ContractAcceptCommand request, CancellationToken ct)
        {
            var incId = (long) request.Command.Data.Options.First().Value;

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var contract = await _mediator.Send(new GetContractQuery(incId));

            var embed = new EmbedBuilder()
                .WithAuthor(contract.Name)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Contracts)));

            if (user.Location != contract.Location)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    $"для работы над этим контрактом необходимо находится в **{contract.Location.Localize(true)}** " +
                    "и быть ничем не занятым.");
            }
            else
            {
                var actionTime = await _mediator.Send(new GetActionTimeQuery(contract.Duration, user.Energy));

                await _mediator.Send(new UpdateUserLocationCommand(user.Id, LocationType.WorkOnContract));
                await _mediator.Send(new CreateUserMovementCommand(
                    user.Id, LocationType.WorkOnContract, contract.Location, DateTimeOffset.UtcNow.Add(actionTime)));
                await _mediator.Send(new CreateUserContractCommand(
                    user.Id, contract.Id, DateTimeOffset.UtcNow.Add(actionTime)));
                await _mediator.Send(new RemoveEnergyFromUserCommand(user.Id, contract.EnergyCost));

                var jobId = BackgroundJob.Schedule<ICompleteContractJob>(
                    x => x.Execute(user.Id, contract.AutoIncrementedId),
                    actionTime);

                await _mediator.Send(new CreateUserHangfireJobCommand(
                    user.Id, HangfireJobType.Contract, jobId, DateTimeOffset.UtcNow.Add(actionTime)));

                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "ты взялся помогать городу выполняя рабочий контракт, это очень здорово. " +
                        "Желаю тебе отлично поработать, не подведи!" +
                        $"\n{StringExtensions.EmptyChar}")
                    .AddField("Ожидаемая награда",
                        $"{emotes.GetEmote(CurrencyType.Ien.ToString())} {contract.CurrencyReward} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), contract.CurrencyReward)}, " +
                        $"{emotes.GetEmote(contract.Location.Reputation().Emote(int.MaxValue))} {contract.ReputationReward} " +
                        $"репутации в **{contract.Location.Localize(true)}**")
                    .AddField("Длительность",
                        actionTime.Humanize(1, new CultureInfo("ru-RU")), true)
                    .AddField("Расход энергии",
                        $"{emotes.GetEmote("Energy")} {contract.EnergyCost} " +
                        $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", contract.EnergyCost)}", true);
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

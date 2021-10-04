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
using Izumi.Services.Game.Calculation;
using Izumi.Services.Game.Contract.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;
using StringExtensions = Izumi.Services.Extensions.StringExtensions;

namespace Izumi.Services.Discord.Commands.Slash.Contract
{
    public record ContractListCommand(SocketSlashCommand Command) : IRequest;

    public class ContractListHandler : IRequestHandler<ContractListCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ContractListHandler(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(ContractListCommand request, CancellationToken ct)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));
            var contracts = await _mediator.Send(new GetContractsInLocationQuery(user.Location));

            var embed = new EmbedBuilder()
                .WithAuthor("Рабочие контракты")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Contracts)));

            if (contracts.Count < 1)
            {
                embed.WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "рабочие контракты доступны в любом крупном городе и только для ничем не занятых путешевственников.");
            }
            else
            {
                embed
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                        "тут отображаются доступные в этой локации рабочие контракты: " +
                        $"\n\n{emotes.GetEmote("Arrow")} Напиши `/контракт` и введи номер рабочего контракта за который хочешь взяться." +
                        $"\n{StringExtensions.EmptyChar}")
                    .WithFooter("* Длительность указана с учетом твоей текущей энергии.");

                foreach (var contract in contracts)
                {
                    var actionTime = await _mediator.Send(new GetActionTimeQuery(contract.Duration, user.Energy));

                    embed.AddField(
                        $"{emotes.GetEmote("List")} `{contract.AutoIncrementedId}` {contract.Name}",
                        $"{contract.Description}" +
                        $"\nНаграда: {emotes.GetEmote(CurrencyType.Ien.ToString())} {contract.CurrencyReward} " +
                        $"{_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Ien.ToString(), contract.CurrencyReward)}, " +
                        $"{emotes.GetEmote(contract.Location.Reputation().Emote(int.MaxValue))} {contract.ReputationReward} " +
                        $"репутации в **{contract.Location.Localize(true)}**" +
                        $"\nДлительность: {actionTime.Humanize(1, new CultureInfo("ru-RU"))}" +
                        $"\nРасход энергии: {emotes.GetEmote("Energy")} {contract.EnergyCost} " +
                        $"{_local.Localize(LocalizationCategoryType.Bar, "Energy", contract.EnergyCost)}");
                }
            }

            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CheckContracts));
            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

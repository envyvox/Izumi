﻿using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Contract
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

            await _mediator.Send(new CheckUserTutorialStepCommand(user.Id, TutorialStepType.CheckContracts));

            return await _mediator.Send(new RespondEmbedCommand(request.Command, new EmbedBuilder()
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Command.User.Mention}, " +
                    "данный функционал находится в разработке. " +
                    "Следи за каналом <#750624435702333460> чтобы быть в курсе всех обновлений.")));
        }
    }
}

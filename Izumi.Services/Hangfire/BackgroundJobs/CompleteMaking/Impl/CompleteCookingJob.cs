using System;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Services.Extensions;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Tutorial.Commands;
using MediatR;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking.Impl
{
    public class CompleteCookingJob : ICompleteCookingJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CompleteCookingJob(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(long userId, Guid foodId, uint amount)
        {
            var emotes = DiscordRepository.Emotes;

            // todo add condition on this check
            await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStepType.CookFriedEgg));
            throw new NotImplementedException();
        }
    }
}

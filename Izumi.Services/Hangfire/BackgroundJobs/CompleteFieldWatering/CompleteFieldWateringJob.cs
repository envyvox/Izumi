using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Game.Field.Commands;
using Izumi.Services.Game.Transit.Commands;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Hangfire.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.CompleteFieldWatering
{
    public class CompleteFieldWateringJob : ICompleteFieldWateringJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CompleteFieldWateringJob> _logger;

        public CompleteFieldWateringJob(
            IMediator mediator,
            ILogger<CompleteFieldWateringJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(long userId)
        {
            _logger.LogInformation(
                "Complete field watering job executed for user {UserId}",
                userId);

            await _mediator.Send(new UpdateUserLocationCommand(userId, LocationType.Village));
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            await _mediator.Send(new UpdateUserFieldsStateCommand(userId, FieldStateType.Watered));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireJobType.FieldWatering));

            var embed = new EmbedBuilder()
                .WithAuthor("Поливка участка земли")
                .WithDescription("Ты успешно полил семена, теперь можно быть уверенным в том, что они будут расти.")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Field)));

            await _mediator.Send(new SendEmbedToUserCommand((ulong) userId, embed));
        }
    }
}

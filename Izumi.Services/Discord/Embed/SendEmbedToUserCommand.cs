using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Embed
{
    public record SendEmbedToUserCommand(ulong UserId, EmbedBuilder Builder, string Message = "") : IRequest;

    public class SendEmbedToUserHandler : IRequestHandler<SendEmbedToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SendEmbedToUserHandler> _logger;

        public SendEmbedToUserHandler(
            IMediator mediator,
            ILogger<SendEmbedToUserHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(SendEmbedToUserCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.UserId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(request.UserId));

            var embed = request.Builder
                .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)))
                .Build();

            try
            {
                await socketUser.SendMessageAsync(request.Message, false, embed);

                _logger.LogInformation(
                    "Sended a direct message to user {UserId}",
                    request.UserId);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Can't send message to user {UserId}",
                    request.UserId);
            }

            return Unit.Value;
        }
    }
}

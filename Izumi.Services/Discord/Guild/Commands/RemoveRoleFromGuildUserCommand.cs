using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Guild.Commands
{
    public record RemoveRoleFromGuildUserCommand(ulong UserId, DiscordRoleType Role) : IRequest;

    public class RemoveRoleFromGuildUserHandler : IRequestHandler<RemoveRoleFromGuildUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RemoveRoleFromGuildUserHandler> _logger;

        public RemoveRoleFromGuildUserHandler(
            IMediator mediator,
            ILogger<RemoveRoleFromGuildUserHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveRoleFromGuildUserCommand request, CancellationToken ct)
        {
            var roles = DiscordRepository.Roles;
            var guild = await _mediator.Send(new GetSocketGuildQuery());
            var user = await _mediator.Send(new GetSocketGuildUserQuery(request.UserId));

            try
            {
                await user.RemoveRoleAsync(guild.GetRole(roles[request.Role].Id));

                _logger.LogInformation(
                    "Removed role {Role} from user {UserId}",
                    request.Role.ToString(), request.UserId);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Can't remove role {Role} from user {UserId}",
                    request.Role.ToString(), request.UserId);
            }

            return Unit.Value;
        }
    }
}

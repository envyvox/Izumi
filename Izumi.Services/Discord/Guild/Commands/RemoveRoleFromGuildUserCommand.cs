using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
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
            var roles = await _mediator.Send(new GetRolesQuery());
            var guild = await _mediator.Send(new GetSocketGuildQuery());
            var user = await _mediator.Send(new GetSocketGuildUserQuery(request.UserId));

            try
            {
                await user.RemoveRoleAsync(guild.GetRole((ulong) roles[request.Role].Id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't remove role from user");
            }

            return Unit.Value;
        }
    }
}

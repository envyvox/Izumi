using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Guild.Commands
{
    public record AddRoleToGuildUserCommand(ulong UserId, DiscordRoleType Role) : IRequest;

    public class AddRoleToGuildUserHandler : IRequestHandler<AddRoleToGuildUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddRoleToGuildUserHandler> _logger;

        public AddRoleToGuildUserHandler(
            IMediator mediator,
            ILogger<AddRoleToGuildUserHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddRoleToGuildUserCommand request, CancellationToken ct)
        {
            var roles = await _mediator.Send(new GetRolesQuery());
            var guild = await _mediator.Send(new GetSocketGuildQuery());
            var user = await _mediator.Send(new GetSocketGuildUserQuery(request.UserId));

            try
            {
                await user.AddRoleAsync(guild.GetRole((ulong) roles[request.Role].Id));

                _logger.LogInformation(
                    "Added role {Role} to user {UserId}",
                    request.Role.ToString(), request.UserId);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Can't add role {Role} to user {UserId}",
                    request.Role.ToString(), request.UserId);

                throw;
            }

            return Unit.Value;
        }
    }
}

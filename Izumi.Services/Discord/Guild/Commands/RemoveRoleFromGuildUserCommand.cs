using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
using MediatR;

namespace Izumi.Services.Discord.Guild.Commands
{
    public record RemoveRoleFromGuildUserCommand(ulong UserId, DiscordRoleType Role) : IRequest;

    public class RemoveRoleFromGuildUserHandler : IRequestHandler<RemoveRoleFromGuildUserCommand>
    {
        private readonly IMediator _mediator;

        public RemoveRoleFromGuildUserHandler(IMediator mediator)
        {
            _mediator = mediator;
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
                Console.WriteLine(e);
                throw;
            }

            return Unit.Value;
        }
    }
}

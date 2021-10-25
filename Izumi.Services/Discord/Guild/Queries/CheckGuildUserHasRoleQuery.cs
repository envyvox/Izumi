using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Extensions;
using MediatR;

namespace Izumi.Services.Discord.Guild.Queries
{
    public record CheckGuildUserHasRoleQuery(ulong UserId, DiscordRoleType Role) : IRequest<bool>;

    public class CheckGuildUserHasRoleHandler : IRequestHandler<CheckGuildUserHasRoleQuery, bool>
    {
        private readonly IMediator _mediator;

        public CheckGuildUserHasRoleHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(CheckGuildUserHasRoleQuery request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetSocketGuildUserQuery(request.UserId));
            var roles = DiscordRepository.Roles;

            return user.Roles.Any(x => x.Id == roles[request.Role].Id);
        }
    }
}

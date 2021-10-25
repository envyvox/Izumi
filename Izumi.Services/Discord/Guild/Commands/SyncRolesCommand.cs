using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Models;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Discord.Guild.Commands
{
    public record SyncRolesCommand : IRequest;

    public class SyncRolesHandler : IRequestHandler<SyncRolesCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SyncRolesHandler> _logger;

        public SyncRolesHandler(
            IMediator mediator,
            ILogger<SyncRolesHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(SyncRolesCommand request, CancellationToken ct)
        {
            var roles = DiscordRepository.Roles;
            var roleTypes = Enum
                .GetValues(typeof(DiscordRoleType))
                .Cast<DiscordRoleType>()
                .ToArray();

            if (roles.Count < roleTypes.Length)
            {
                var guild = await _mediator.Send(new GetSocketGuildQuery());

                foreach (var roleType in roleTypes)
                {
                    if (roles.ContainsKey(roleType)) continue;

                    var roleInGuild = guild.Roles.FirstOrDefault(x => x.Name == roleType.Name());
                    ulong roleId;

                    if (roleInGuild is null)
                    {
                        var newRole = await guild.CreateRoleAsync(
                            name: roleType.Name(),
                            permissions: null,
                            color: new Color(uint.Parse(roleType.Color(), NumberStyles.HexNumber)),
                            isHoisted: false,
                            options: null);

                        roleId = newRole.Id;
                    }
                    else
                    {
                        roleId = roleInGuild.Id;
                    }

                    roles.Add(roleType, new RoleDto(roleId, roleType));
                }
            }

            _logger.LogInformation(
                "Roles sync completed");

            return Unit.Value;
        }
    }
}

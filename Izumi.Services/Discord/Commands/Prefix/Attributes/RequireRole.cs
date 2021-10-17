using System;
using System.Threading.Tasks;
using Discord.Commands;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Izumi.Services.Discord.Commands.Prefix.Attributes
{
    public class RequireRole : PreconditionAttribute
    {
        private readonly DiscordRoleType _role;

        public RequireRole(DiscordRoleType role)
        {
            _role = role;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var service = services.GetRequiredService<IMediator>();
            var hasRole = await service.Send(new CheckGuildUserHasRoleQuery(
                context.User.Id, _role));
            var hasAdminRole = await service.Send(new CheckGuildUserHasRoleQuery(
                context.User.Id, DiscordRoleType.Administration));

            return hasRole
                ? PreconditionResult.FromSuccess()
                : hasAdminRole
                    ? PreconditionResult.FromSuccess()
                    : PreconditionResult.FromError(
                        $"Эта команда доступна только для пользователей с ролью **{_role.Name()}**");
        }
    }
}

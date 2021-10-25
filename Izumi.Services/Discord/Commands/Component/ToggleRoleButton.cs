using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using MediatR;

namespace Izumi.Services.Discord.Commands.Component
{
    public record ToggleRoleButton(SocketMessageComponent Component) : IRequest;

    public class ToggleRoleButtonHandler : IRequestHandler<ToggleRoleButton>
    {
        private readonly IMediator _mediator;

        public ToggleRoleButtonHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ToggleRoleButton request, CancellationToken ct)
        {
            var roles = DiscordRepository.Roles;
            var role = request.Component.Data.CustomId switch
            {
                // роль мероприятий
                "toggle-role-DiscordEvent" => DiscordRoleType.DiscordEvent,
                _ => throw new ArgumentOutOfRangeException()
            };

            var hasRole = await _mediator.Send(new CheckGuildUserHasRoleQuery(request.Component.User.Id, role));

            var embed = new EmbedBuilder()
                .WithDefaultColor();

            if (hasRole)
            {
                await _mediator.Send(new RemoveRoleFromGuildUserCommand(request.Component.User.Id, role));

                embed.WithDescription($"Ты успешно отказался от роли <@&{roles[role].Id}>.");

                await request.Component.FollowupAsync(embed: embed.Build(), ephemeral: true);
            }
            else
            {
                await _mediator.Send(new AddRoleToGuildUserCommand(request.Component.User.Id, role));

                embed.WithDescription($"Ты успешно получил роль <@&{roles[role].Id}>.");

                await request.Component.FollowupAsync(embed: embed.Build(), ephemeral: true);
            }

            return Unit.Value;
        }
    }
}

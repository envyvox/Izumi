using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Queries;
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
            // ALL THIS CODE IS NOT NEEDED CURRENTLY
            // BUT MAYBE THERE WILL BE SOME BUTTONS IN FUTURE, SO

            var role = request.Component.Data.CustomId switch
            {
                "..." => DiscordRoleType.Administration,
                _ => throw new ArgumentOutOfRangeException()
            };

            var roles = DiscordRepository.Roles;
            var user = await _mediator.Send(new GetUserQuery((long) request.Component.User.Id));
            var hasRole = await _mediator.Send(new CheckGuildUserHasRoleQuery(request.Component.User.Id, role));

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)));

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
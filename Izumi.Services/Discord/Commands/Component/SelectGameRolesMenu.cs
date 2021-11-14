using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public record SelectGameRolesMenu(SocketMessageComponent Component) : IRequest;

    public class SelectGameRolesMenuHandler : IRequestHandler<SelectGameRolesMenu>
    {
        private readonly IMediator _mediator;

        private readonly IReadOnlyCollection<string> _gameRolesNames = new[]
        {
            DiscordRoleType.GenshinImpact.Name(),
            DiscordRoleType.LeagueOfLegends.Name(),
            DiscordRoleType.TeamfightTactics.Name(),
            DiscordRoleType.Valorant.Name(),
            DiscordRoleType.ApexLegends.Name(),
            DiscordRoleType.Dota.Name(),
            DiscordRoleType.Minecraft.Name(),
            DiscordRoleType.Osu.Name(),
            DiscordRoleType.AmongUs.Name(),
            DiscordRoleType.Rust.Name(),
            DiscordRoleType.CsGo.Name(),
            DiscordRoleType.HotS.Name(),
            DiscordRoleType.NewWorld.Name(),
            DiscordRoleType.MobileGaming.Name()
        };

        public SelectGameRolesMenuHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SelectGameRolesMenu request, CancellationToken ct)
        {
            var roles = DiscordRepository.Roles;
            var selectedValues = request.Component.Data.Values;
            var guildUser = await _mediator.Send(new GetSocketGuildUserQuery(request.Component.User.Id));
            var user = await _mediator.Send(new GetUserQuery((long) request.Component.User.Id));
            var userRoles = guildUser.Roles.Where(x => _gameRolesNames.Contains(x.Name)).ToArray();

            var selectedRoles = selectedValues.Select(selectedValue => selectedValue switch
                {
                    "GenshinImpact" => DiscordRoleType.GenshinImpact,
                    "LeagueOfLegends" => DiscordRoleType.LeagueOfLegends,
                    "TeamfightTactics" => DiscordRoleType.TeamfightTactics,
                    "Valorant" => DiscordRoleType.Valorant,
                    "ApexLegends" => DiscordRoleType.ApexLegends,
                    "Dota" => DiscordRoleType.Dota,
                    "Minecraft" => DiscordRoleType.Minecraft,
                    "Osu" => DiscordRoleType.Osu,
                    "AmongUs" => DiscordRoleType.AmongUs,
                    "Rust" => DiscordRoleType.Rust,
                    "CSGO" => DiscordRoleType.CsGo,
                    "HotS" => DiscordRoleType.HotS,
                    "NewWorld" => DiscordRoleType.NewWorld,
                    "MobileGaming" => DiscordRoleType.MobileGaming,
                    _ => throw new ArgumentOutOfRangeException()
                })
                .Select(x => x.Name())
                .ToList();

            var rolesToRemove = userRoles.Where(x => !selectedRoles.Contains(x.Name));
            var rolesToAdd = selectedRoles.Where(x => userRoles.All(r => r.Name != x));

            var addedRoles = string.Empty;
            var removedRoles = string.Empty;

            foreach (var role in rolesToRemove)
            {
                var roleDto = roles.Values.SingleOrDefault(x => x.Id == role.Id);

                if (roleDto is null)
                {
                    throw new Exception($"role with id {role.Id} not found in roles dict");
                }

                await _mediator.Send(new RemoveRoleFromGuildUserCommand(guildUser.Id, roleDto.Type));

                removedRoles += $"<@&{roleDto.Id}>, ";
            }

            foreach (var roleName in rolesToAdd)
            {
                var roleDto = roles.Values.SingleOrDefault(x => x.Type.Name() == roleName);

                if (roleDto is null)
                {
                    throw new Exception($"role with name {roleName} not found in roles dict");
                }

                await _mediator.Send(new AddRoleToGuildUserCommand(guildUser.Id, roleDto.Type));

                addedRoles += $"<@&{roleDto.Id}>, ";
            }

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)))
                .WithDescription(
                    (addedRoles.Length > 0
                        ? $"Ты успешно получил роли: {addedRoles.RemoveFromEnd(2)}\n"
                        : "") +
                    (removedRoles.Length > 0
                        ? $"Ты успешно снял роли: {removedRoles.RemoveFromEnd(2)}"
                        : ""));

            await request.Component.FollowupAsync(embed: embed.Build(), ephemeral: true);

            return Unit.Value;
        }
    }
}
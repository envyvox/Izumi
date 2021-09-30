using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
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
            var roles = await _mediator.Send(new GetRolesQuery());
            var role = request.Component.Data.CustomId switch
            {
                // роль мероприятий
                "toggle-role-DiscordEvent" => DiscordRoleType.DiscordEvent,
                // роли оповещений событий
                "toggle-role-AllEvents" => DiscordRoleType.AllEvents,
                "toggle-role-DailyEvents" => DiscordRoleType.DailyEvents,
                "toggle-role-WeeklyEvents" => DiscordRoleType.WeeklyEvents,
                "toggle-role-MonthlyEvents" => DiscordRoleType.MonthlyEvents,
                "toggle-role-YearlyEvents" => DiscordRoleType.YearlyEvents,
                "toggle-role-UniqueEvents" => DiscordRoleType.UniqueEvents,
                // игровые роли
                "toggle-role-GenshinImpact" => DiscordRoleType.GenshinImpact,
                "toggle-role-LeagueOfLegends" => DiscordRoleType.LeagueOfLegends,
                "toggle-role-TeamfightTactics" => DiscordRoleType.TeamfightTactics,
                "toggle-role-Valorant" => DiscordRoleType.Valorant,
                "toggle-role-ApexLegends" => DiscordRoleType.ApexLegends,
                "toggle-role-LostArk" => DiscordRoleType.LostArk,
                "toggle-role-Dota" => DiscordRoleType.Dota,
                "toggle-role-Osu" => DiscordRoleType.Osu,
                "toggle-role-AmongUs" => DiscordRoleType.AmongUs,
                "toggle-role-Rust" => DiscordRoleType.Rust,
                "toggle-role-CSGO" => DiscordRoleType.CsGo,
                "toggle-role-HotS" => DiscordRoleType.HotS,
                "toggle-role-WildRift" => DiscordRoleType.WildRift,
                "toggle-role-MobileLegends" => DiscordRoleType.MobileLegends,
                _ => throw new ArgumentOutOfRangeException()
            };

            var hasRole = await _mediator.Send(new CheckGuildUserHasRoleQuery(request.Component.User.Id, role));

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse("202225", NumberStyles.HexNumber)));

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

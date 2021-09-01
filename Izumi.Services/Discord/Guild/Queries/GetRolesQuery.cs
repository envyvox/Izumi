using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Discord;
using Izumi.Data;
using Izumi.Data.Enums.Discord;
using Izumi.Data.Extensions;
using Izumi.Services.Discord.Guild.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.Guild.Queries
{
    public record GetRolesQuery : IRequest<Dictionary<DiscordRoleType, RoleDto>>;

    public class GetRolesHandler : IRequestHandler<GetRolesQuery, Dictionary<DiscordRoleType, RoleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public GetRolesHandler(
            DbContextOptions options,
            IMapper mapper,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Dictionary<DiscordRoleType, RoleDto>> Handle(GetRolesQuery request, CancellationToken ct)
        {
            var roles = await _db.Roles
                .AsQueryable()
                .ToDictionaryAsync(x => x.Type);

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
                    long roleId;

                    if (roleInGuild is null)
                    {
                        var newRole = await guild.CreateRoleAsync(
                            name: roleType.Name(),
                            permissions: null,
                            color: new Color(uint.Parse(roleType.Color(), NumberStyles.HexNumber)),
                            isHoisted: false,
                            options: null);

                        roleId = (long) newRole.Id;
                    }
                    else
                    {
                        roleId = (long) roleInGuild.Id;
                    }

                    roles.Add(roleType, new Data.Entities.Discord.Role
                    {
                        Id = roleId,
                        Type = roleType
                    });

                    await _db.CreateEntity(new Data.Entities.Discord.Role
                    {
                        Id = roleId,
                        Type = roleType
                    });
                }
            }

            return _mapper.Map<Dictionary<DiscordRoleType, RoleDto>>(roles);
        }
    }
}

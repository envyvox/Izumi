using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record UserVoiceStateUpdated(
            SocketUser SocketUser,
            SocketVoiceState OldVoiceState,
            SocketVoiceState NewVoiceState)
        : IRequest;

    public class UserVoiceStateUpdatedHandler : IRequestHandler<UserVoiceStateUpdated>
    {
        private readonly IMediator _mediator;

        public UserVoiceStateUpdatedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UserVoiceStateUpdated request, CancellationToken cancellationToken)
        {
            var channels = await _mediator.Send(new GetChannelsQuery());
            var roles = await _mediator.Send(new GetRolesQuery());

            var createRoomParent = (ulong) channels[DiscordChannelType.CreateRoomParent].Id;
            var createRoom = (ulong) channels[DiscordChannelType.CreateRoom].Id;

            var eventParent = (ulong) channels[DiscordChannelType.EventParent].Id;
            var eventCreateRoom = (ulong) channels[DiscordChannelType.EventCreateRoom].Id;

            var oldChannel = request.OldVoiceState.VoiceChannel;
            var newChannel = request.NewVoiceState.VoiceChannel;

            if (oldChannel is null)
            {
                await _mediator.Send(new AddRoleToGuildUserCommand(
                    request.SocketUser.Id, DiscordRoleType.InVoice));
            }

            if (newChannel is null)
            {
                await _mediator.Send(new RemoveRoleFromGuildUserCommand(
                    request.SocketUser.Id, DiscordRoleType.InVoice));
            }

            if (newChannel?.Id == createRoom)
            {
                var createdChannel = await newChannel.Guild.CreateVoiceChannelAsync(request.SocketUser.Username, x =>
                {
                    x.CategoryId = createRoomParent;
                    x.UserLimit = 5;
                });

                await newChannel.Guild
                    .GetUser(request.SocketUser.Id)
                    .ModifyAsync(x => x.Channel = createdChannel);

                await createdChannel.AddPermissionOverwriteAsync(request.SocketUser,
                    new OverwritePermissions(manageChannel: PermValue.Allow));
            }

            if (newChannel?.Id == eventCreateRoom)
            {
                var createdChannel = await newChannel.Guild.CreateVoiceChannelAsync("Мероприятие",
                    x => x.CategoryId = eventParent);

                await newChannel.Guild
                    .GetUser(request.SocketUser.Id)
                    .ModifyAsync(x => x.Channel = createdChannel);

                var eventManagerRole = newChannel.Guild.GetRole((ulong) roles[DiscordRoleType.EventManager].Id);
                var moderatorRole = newChannel.Guild.GetRole((ulong) roles[DiscordRoleType.Moderator].Id);

                await createdChannel.AddPermissionOverwriteAsync(eventManagerRole,
                    new OverwritePermissions(
                        createInstantInvite: PermValue.Allow,
                        manageChannel: PermValue.Allow,
                        manageRoles: PermValue.Allow,
                        viewChannel: PermValue.Allow,
                        connect: PermValue.Allow,
                        speak: PermValue.Allow,
                        muteMembers: PermValue.Allow,
                        deafenMembers: PermValue.Allow,
                        moveMembers: PermValue.Allow,
                        useVoiceActivation: PermValue.Allow,
                        prioritySpeaker: PermValue.Allow,
                        stream: PermValue.Allow));

                await createdChannel.AddPermissionOverwriteAsync(moderatorRole,
                    new OverwritePermissions(
                        createInstantInvite: PermValue.Allow,
                        manageChannel: PermValue.Deny,
                        manageRoles: PermValue.Deny,
                        viewChannel: PermValue.Allow,
                        connect: PermValue.Allow,
                        speak: PermValue.Allow,
                        muteMembers: PermValue.Allow,
                        deafenMembers: PermValue.Allow,
                        moveMembers: PermValue.Allow,
                        useVoiceActivation: PermValue.Allow,
                        prioritySpeaker: PermValue.Allow,
                        stream: PermValue.Allow));

                await createdChannel.AddPermissionOverwriteAsync(newChannel.Guild.EveryoneRole,
                    new OverwritePermissions(
                        createInstantInvite: PermValue.Allow,
                        manageChannel: PermValue.Deny,
                        manageRoles: PermValue.Deny,
                        viewChannel: PermValue.Allow,
                        connect: PermValue.Allow,
                        speak: PermValue.Allow,
                        muteMembers: PermValue.Deny,
                        deafenMembers: PermValue.Deny,
                        moveMembers: PermValue.Deny,
                        useVoiceActivation: PermValue.Allow,
                        prioritySpeaker: PermValue.Deny,
                        stream: PermValue.Deny));
            }

            if (oldChannel?.CategoryId == createRoomParent &&
                oldChannel.Users.Count == 0 &&
                oldChannel.Id != createRoom)
            {
                await oldChannel.DeleteAsync();
            }

            if (oldChannel?.CategoryId == eventParent &&
                oldChannel.Users.Count == 0 &&
                oldChannel.Id != eventCreateRoom)
            {
                await oldChannel.DeleteAsync();
            }

            return Unit.Value;
        }
    }
}

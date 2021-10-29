using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Voice.Commands;
using Izumi.Services.Extensions;
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
            var channels = DiscordRepository.Channels;
            var roles = DiscordRepository.Roles;

            var createRoomParent = channels[DiscordChannelType.CreateRoomParent].Id;
            var createRoom = channels[DiscordChannelType.CreateRoom].Id;
            var eventParent = channels[DiscordChannelType.EventParent].Id;
            var eventCreateRoom = channels[DiscordChannelType.EventCreateRoom].Id;
            var familyParent = channels[DiscordChannelType.FamilyRoomParent].Id;
            var afkRoom = channels[DiscordChannelType.Afk].Id;

            var oldChannel = request.OldVoiceState.VoiceChannel;
            var newChannel = request.NewVoiceState.VoiceChannel;

            if (oldChannel is null && newChannel.Id != afkRoom)
            {
                await _mediator.Send(new AddRoleToGuildUserCommand(
                    request.SocketUser.Id, DiscordRoleType.InVoice));
                await _mediator.Send(new CreateUserVoiceCommand(
                    (long) request.SocketUser.Id, (long) newChannel.Id));
            }

            if (newChannel is null || newChannel.Id == afkRoom)
            {
                await _mediator.Send(new RemoveRoleFromGuildUserCommand(
                    request.SocketUser.Id, DiscordRoleType.InVoice));
                await _mediator.Send(new DeleteUserVoiceCommand(
                    (long) request.SocketUser.Id));
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

                var eventManagerRole = newChannel.Guild.GetRole(roles[DiscordRoleType.EventManager].Id);
                var moderatorRole = newChannel.Guild.GetRole(roles[DiscordRoleType.Moderator].Id);

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

            if (newChannel?.CategoryId == familyParent &&
                newChannel.Users.Count == 1)
            {
                await newChannel.AddPermissionOverwriteAsync(newChannel.Guild.EveryoneRole,
                    new OverwritePermissions(
                        createInstantInvite: PermValue.Deny,
                        manageChannel: PermValue.Deny,
                        manageRoles: PermValue.Deny,
                        viewChannel: PermValue.Allow,
                        connect: PermValue.Deny,
                        speak: PermValue.Allow,
                        muteMembers: PermValue.Deny,
                        deafenMembers: PermValue.Deny,
                        moveMembers: PermValue.Deny,
                        useVoiceActivation: PermValue.Allow,
                        prioritySpeaker: PermValue.Deny,
                        stream: PermValue.Allow,
                        startEmbeddedActivities: PermValue.Allow));
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

            if (oldChannel?.CategoryId == familyParent &&
                oldChannel.Users.Count == 0)
            {
                await oldChannel.AddPermissionOverwriteAsync(oldChannel.Guild.EveryoneRole,
                    new OverwritePermissions(
                        createInstantInvite: PermValue.Deny,
                        manageChannel: PermValue.Deny,
                        manageRoles: PermValue.Deny,
                        viewChannel: PermValue.Deny,
                        connect: PermValue.Deny,
                        speak: PermValue.Allow,
                        muteMembers: PermValue.Deny,
                        deafenMembers: PermValue.Deny,
                        moveMembers: PermValue.Deny,
                        useVoiceActivation: PermValue.Allow,
                        prioritySpeaker: PermValue.Deny,
                        stream: PermValue.Allow,
                        startEmbeddedActivities: PermValue.Allow));
            }

            return Unit.Value;
        }
    }
}

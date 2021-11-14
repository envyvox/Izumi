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

            var createRoomParent = channels[DiscordChannelType.CreateRoomParent].Id;
            var createRoom = channels[DiscordChannelType.CreateRoom].Id;
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
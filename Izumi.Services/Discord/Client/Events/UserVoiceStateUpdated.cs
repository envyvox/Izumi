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

            var createRoomParent = (ulong) channels[DiscordChannelType.CreateRoomParent].Id;
            var createRoom = (ulong) channels[DiscordChannelType.CreateRoom].Id;

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
                    .ModifyAsync(x => { x.Channel = createdChannel; });

                await createdChannel.AddPermissionOverwriteAsync(request.SocketUser,
                    new OverwritePermissions(manageChannel: PermValue.Allow));
            }

            if (oldChannel?.CategoryId == createRoomParent &&
                oldChannel.Users.Count == 0 &&
                oldChannel.Id != createRoom)
            {
                await oldChannel.DeleteAsync();
            }

            return Unit.Value;
        }
    }
}

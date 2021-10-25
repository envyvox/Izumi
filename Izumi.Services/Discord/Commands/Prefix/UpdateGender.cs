using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Commands.Prefix.Attributes;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Prefix
{
    [RequireRole(DiscordRoleType.Moderator)]
    public class UpdateGender : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UpdateGender(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("update-gender")]
        public async Task UpdateGenderTask(IUser mentionedUser, GenderType gender)
        {
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) mentionedUser.Id));

            await _mediator.Send(new UpdateUserGenderCommand(user.Id, gender));
            await _mediator.Send(new AddRoleToGuildUserCommand(mentionedUser.Id, gender.Role()));

            var embed = new EmbedBuilder()
                .WithDescription(
                    $"Пол {mentionedUser.Mention} обновлен на {emotes.GetEmote(gender.EmoteName())} {gender.Localize()}.");

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannelType.Moderation, embed));

            var notify = new EmbedBuilder()
                .WithDescription(
                    $"Твой пол был обновлен на {emotes.GetEmote(gender.EmoteName())} {gender.Localize()}.");

            await _mediator.Send(new SendEmbedToUserCommand(mentionedUser.Id, notify));
        }
    }
}

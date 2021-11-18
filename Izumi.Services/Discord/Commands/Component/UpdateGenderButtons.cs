using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Extensions;
using Izumi.Services.Game.User.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Component
{
    public record UpdateGenderButtons(SocketMessageComponent Component) : IRequest;

    public class UpdateGenderButtonsHandler : IRequestHandler<UpdateGenderButtons>
    {
        private readonly IMediator _mediator;

        public UpdateGenderButtonsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateGenderButtons request, CancellationToken ct)
        {
            var gender = request.Component.Data.CustomId.Contains("male")
                ? GenderType.Male
                : GenderType.Female;
            var userId = ulong.Parse(Regex.Match(request.Component.Data.CustomId, @"\d+").Value);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) userId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(userId));

            await _mediator.Send(new UpdateUserGenderCommand(user.Id, gender));
            await _mediator.Send(new AddRoleToGuildUserCommand(socketUser.Id, gender.Role()));

            var embed = new EmbedBuilder()
                .WithDefaultColor()
                .WithDescription(
                    $"Пол {socketUser.Mention} обновлен на {emotes.GetEmote(gender.EmoteName())} {gender.Localize()}.");

            await request.Component.FollowupAsync(embed: embed.Build());

            var notify = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithDescription(
                    $"Твой пол был обновлен на {emotes.GetEmote(gender.EmoteName())} {gender.Localize()}.");

            await _mediator.Send(new SendEmbedToUserCommand(socketUser.Id, notify));

            return Unit.Value;
        }
    }
}
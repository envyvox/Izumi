using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Commands.Slash.User;
using Izumi.Services.Discord.Commands.Slash.User.Info;
using Izumi.Services.Discord.Commands.Slash.User.Info.Interaction;
using Izumi.Services.Discord.Commands.Slash.User.Referral;
using Izumi.Services.Discord.Commands.Slash.User.Transit;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Client.Events
{
    public record InteractionCreated(SocketInteraction Interaction) : IRequest;

    public class InteractionCreatedHandler : IRequestHandler<InteractionCreated>
    {
        private readonly IMediator _mediator;

        public InteractionCreatedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(InteractionCreated request, CancellationToken ct)
        {
            await request.Interaction.DeferAsync(true, new RequestOptions
            {
                RetryMode = RetryMode.Retry502,
                Timeout = 10000
            });
            try
            {
                return request.Interaction switch
                {
                    SocketSlashCommand command => command.Data.Name switch
                    {
                        "доска-сообщества" => await _mediator.Send(new CommunityDescCommand(command)),
                        "мир" => await _mediator.Send(new WorldInfoCommand(command)),
                        "профиль" => await _mediator.Send(new ProfileCommand(command)),
                        "информация" => await _mediator.Send(new UpdateAboutCommand(command)),
                        "инвентарь" => await _mediator.Send(new InventoryCommand(command)),
                        "титулы" => await _mediator.Send(new TitlesCommand(command)),
                        "титул" => await _mediator.Send(new UpdateTitleCommand(command)),
                        "баннеры" => await _mediator.Send(new BannersCommand(command)),
                        "баннер" => await _mediator.Send(new UpdateBannerCommand(command)),
                        "приглашения" => await _mediator.Send(new ReferralListCommand(command)),
                        "пригласил" => await _mediator.Send(new ReferralSetCommand(command)),
                        "отправления" => await _mediator.Send(new TransitListCommand(command)),
                        "отправиться" => await _mediator.Send(new TransitMakeCommand(command)),
                        _ => Unit.Value
                    },
                    SocketMessageComponent component => component.Data.CustomId switch
                    {
                        _ => Unit.Value
                    },
                    _ => Unit.Value
                };
            }
            catch (Exception e)
            {
                var emotes = await _mediator.Send(new GetEmotesQuery());
                var user = await _mediator.Send(new GetUserQuery((long) request.Interaction.User.Id));

                var embed = new EmbedBuilder()
                    .WithColor(new Color(uint.Parse("202225", NumberStyles.HexNumber)))
                    .WithAuthor("Ой, кажется что-то пошло не так...")
                    .WithDescription(
                        $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {request.Interaction.User.Mention}, {e.Message}")
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.CommandError)));

                await request.Interaction.FollowupAsync("", new[] { embed.Build() }, false, true);
            }

            return Unit.Value;
        }
    }
}

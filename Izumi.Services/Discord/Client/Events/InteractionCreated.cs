using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Commands.Slash.User.Info;
using Izumi.Services.Discord.Image.Queries;
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
                        "профиль" => await _mediator.Send(new ProfileCommand(command)),
                        "доска-сообщества" => await _mediator.Send(new CommunityDescCommand(command)),
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
                var embed = new EmbedBuilder()
                    .WithColor(new Color(uint.Parse("202225", NumberStyles.HexNumber)))
                    .WithAuthor("Ой, кажется что-то пошло не так...")
                    .WithDescription(e.Message)
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.CommandError)));

                await request.Interaction.FollowupAsync("", new[] { embed.Build() }, false, true);
            }

            return Unit.Value;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.SlashCommands.Commands.Administration;
using Izumi.Services.Discord.SlashCommands.Commands.User.Info;
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
                        "ping" => await _mediator.Send(new PingCommand(command)),
                        "профиль" => await _mediator.Send(new ProfileCommand(command)),
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
                    .WithAuthor("Ой, кажется что-то пошло не так...")
                    .WithDescription(e.Message);

                await _mediator.Send(new RespondEmbedCommand((SocketSlashCommand) request.Interaction, embed, true));
            }

            return Unit.Value;
        }
    }
}

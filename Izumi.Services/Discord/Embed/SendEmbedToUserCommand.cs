using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Embed
{
    public record SendEmbedToUserCommand(ulong UserId, EmbedBuilder Builder, string Message = "") : IRequest;

    public class SendEmbedToUserHandler : IRequestHandler<SendEmbedToUserCommand>
    {
        private readonly IMediator _mediator;

        public SendEmbedToUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendEmbedToUserCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.UserId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(request.UserId));

            var embed = request.Builder
                .WithColor(new Color(uint.Parse(user.CommandColor, NumberStyles.HexNumber)))
                .Build();

            try
            {
                await socketUser.SendMessageAsync(request.Message, false, embed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Unit.Value;
        }
    }
}

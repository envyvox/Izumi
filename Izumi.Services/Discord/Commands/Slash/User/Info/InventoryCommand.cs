using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Izumi.Data.Enums;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Image.Queries;
using Izumi.Services.Game.Box.Queries;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Currency.Queries;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.Seed.Queries;
using Izumi.Services.Game.User.Queries;
using MediatR;

namespace Izumi.Services.Discord.Commands.Slash.User.Info
{
    public record InventoryCommand(SocketSlashCommand Command) : IRequest;

    public class InventoryHandler : IRequestHandler<InventoryCommand>
    {
        private readonly IMediator _mediator;

        public InventoryHandler(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(InventoryCommand request, CancellationToken ct)
        {
            var type = request.Command.Data.Options?.First().Value;
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var user = await _mediator.Send(new GetUserQuery((long) request.Command.User.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Инвентарь")
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(ImageType.Inventory)));

            if (type is null)
            {
                var userCurrencies = await _mediator.Send(new GetUserCurrenciesQuery(user.Id));
                var userBoxes = await _mediator.Send(new GetUserBoxesQuery(user.Id));
                var userGatherings = await _mediator.Send(new GetUserGatheringsQuery(user.Id));
                var userCraftings = await _mediator.Send(new GetUserCraftingsQuery(user.Id));
                var userProducts = await _mediator.Send(new GetUserProductsQuery(user.Id));
                var userSeeds = await _mediator.Send(new GetUserSeedsQuery(user.Id));
                var userCrops = await _mediator.Send(new GetUserCropsQuery(user.Id));
                var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));
                var userFoods = await _mediator.Send(new GetUserFoodsQuery(user.Id));
            }
            else
            {
                switch (type)
                {
                    case "рыба":

                        var userFishes = await _mediator.Send(new GetUserFishesQuery(user.Id));

                        break;
                    case "семена":

                        var userSeeds = await _mediator.Send(new GetUserSeedsQuery(user.Id));

                        break;
                    case "урожай":

                        var userCrops = await _mediator.Send(new GetUserCropsQuery(user.Id));

                        break;
                }
            }

            return await _mediator.Send(new RespondEmbedCommand(request.Command, embed));
        }
    }
}

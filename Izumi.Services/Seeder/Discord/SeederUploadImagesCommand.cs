using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Discord.Image.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Discord
{
    public record SeederUploadImagesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadImagesHandler : IRequestHandler<SeederUploadImagesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadImagesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadImagesCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateImageCommand[]
            {
                new(ImageType.WeatherClear, "https://cdn.discordapp.com/attachments/769277306899791882/769277341053878293/weather_clear.gif"),
                new(ImageType.WeatherRain, "https://cdn.discordapp.com/attachments/769277306899791882/769277342173626408/weather_rain.gif"),
                new(ImageType.ExploreCastle, "https://cdn.discordapp.com/attachments/771030295713415209/792232646339461131/ExploreCastle.png"),
                new(ImageType.ExploreGarden, "https://cdn.discordapp.com/attachments/771030295713415209/792232622960148550/ExploreGarden.png"),
                new(ImageType.Fishing, "https://cdn.discordapp.com/attachments/771030295713415209/792552462241628160/Fishing.png"),
                new(ImageType.InTransit, "https://cdn.discordapp.com/attachments/771030295713415209/792552489618112552/InTransit.png"),
                new(ImageType.Casino, "https://cdn.discordapp.com/attachments/771030295713415209/790605891552018462/CapitalCasino.png"),
                new(ImageType.Market, "https://cdn.discordapp.com/attachments/771030295713415209/791706561721663488/CapitalMarket.png"),
                new(ImageType.ShopProduct, "https://cdn.discordapp.com/attachments/790988929795883008/791009242328465438/VillageProductShop.png"),
                new(ImageType.ShopBanner, "https://cdn.discordapp.com/attachments/790988929795883008/793977646790869002/CapitalBannerShop.png"),
                new(ImageType.ShopFisher, "https://cdn.discordapp.com/attachments/790988929795883008/791010430155096094/SeaportFisherShop.png"),
                new(ImageType.ShopRecipe, "https://cdn.discordapp.com/attachments/790988929795883008/791032981103837204/GardenRecipeShop.png"),
                new(ImageType.ShopSeed, "https://cdn.discordapp.com/attachments/790988929795883008/843610297247858708/CapitalSeedShop.png"),
                new(ImageType.Cooking, "https://cdn.discordapp.com/attachments/791043681591230504/791043718227427388/Cooking.png"),
                new(ImageType.Crafting, "https://cdn.discordapp.com/attachments/791043681591230504/791337113806700574/Crafting.png"),
                new(ImageType.Inventory, "https://cdn.discordapp.com/attachments/791043681591230504/791423844513742868/Inventory.png"),
                new(ImageType.TransitList, "https://cdn.discordapp.com/attachments/791043681591230504/791423870153785344/Transit.png"),
                new(ImageType.Collection, "https://cdn.discordapp.com/attachments/791043681591230504/791429340524445696/Collection.png"),
                new(ImageType.WorldInfo, "https://cdn.discordapp.com/attachments/791043681591230504/791465602022768650/WorldInfo.png"),
                new(ImageType.Achievements, "https://cdn.discordapp.com/attachments/791043681591230504/793977267147374602/Achievements.png"),
                new(ImageType.Contracts, "https://cdn.discordapp.com/attachments/791043681591230504/820256452269965352/Contract.png"),
                new(ImageType.Field, "https://cdn.discordapp.com/attachments/791043681591230504/843610587937112084/HarvestingField.png"),
                new(ImageType.CommandError, "https://cdn.discordapp.com/attachments/836171517649616956/836171702119170078/Panda1.png"),
                new(ImageType.CommandError, "https://cdn.discordapp.com/attachments/836171517649616956/836171704219992064/Panda2.png"),
                new(ImageType.CommandError, "https://cdn.discordapp.com/attachments/836171517649616956/836171706016071680/Panda3.png"),
                new(ImageType.GetEventRole, "https://cdn.discordapp.com/attachments/842067362139209778/850652843848761394/unknown.png"),
                new(ImageType.ColorPicker, "https://cdn.discordapp.com/attachments/842067362139209778/848897929641328660/ColorPicker.png"),
                new(ImageType.Premium, "https://cdn.discordapp.com/attachments/842067362139209778/848626579978190868/Premium.png"),
                new(ImageType.GetGameRoles, "https://cdn.discordapp.com/attachments/842067362139209778/898169768266309662/unknown.png"),
                new(ImageType.RequestGenderRole, "https://cdn.discordapp.com/attachments/842067362139209778/910949110935330826/unknown.png")
            };

            foreach (var createImageCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createImageCommand);

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}

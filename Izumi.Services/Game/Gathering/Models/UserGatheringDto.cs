namespace Izumi.Services.Game.Gathering.Models
{
    public record UserGatheringDto(
        Data.Entities.Resource.Gathering Gathering,
        uint Amount);
}

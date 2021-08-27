namespace Izumi.Services.Game.Seafood.Models
{
    public record UserSeafoodDto(
        Data.Entities.Resource.Seafood Seafood,
        uint Amount);
}

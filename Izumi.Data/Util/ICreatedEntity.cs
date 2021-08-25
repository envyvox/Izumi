using System;

namespace Izumi.Data.Util
{
    public interface ICreatedEntity
    {
        DateTimeOffset CreatedAt { get; set; }
    }
}

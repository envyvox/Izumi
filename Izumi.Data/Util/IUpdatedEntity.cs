using System;

namespace Izumi.Data.Util
{
    public interface IUpdatedEntity
    {
        DateTimeOffset UpdatedAt { get; set; }
    }
}

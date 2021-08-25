using System;

namespace Izumi.Data.Util
{
    public interface IUniqueIdentifiedEntity
    {
        Guid Id { get; set; }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Izumi.Data.Converters
{
    public class DateTimeUtcKindConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeUtcKindConverter() : base(x => x, x => DateTime.SpecifyKind(x, DateTimeKind.Utc))
        {
        }
    }
}

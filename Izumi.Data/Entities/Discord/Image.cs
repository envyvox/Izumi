using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Discord
{
    public class Image : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public ImageType Type { get; set; }
        public string Url { get; set; }
    }

    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Type);

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Url).IsRequired();
        }
    }
}

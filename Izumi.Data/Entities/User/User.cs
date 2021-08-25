using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class User : ICreatedEntity, IUpdatedEntity
    {
        public long Id { get; set; }
        public string About { get; set; }
        public TitleType Title { get; set; }
        public GenderType Gender { get; set; }
        public LocationType Location { get; set; }
        public uint Energy { get; set; }
        public uint Points { get; set; }
        public bool IsPremium { get; set; }
        public string CommandColor { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.About);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Gender).IsRequired();
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.Energy).IsRequired();
            builder.Property(x => x.Points).IsRequired();
            builder.Property(x => x.IsPremium).IsRequired();
            builder.Property(x => x.CommandColor).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
        }
    }
}

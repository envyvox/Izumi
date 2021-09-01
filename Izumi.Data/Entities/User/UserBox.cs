using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class UserBox : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public BoxType Box { get; set; }
        public uint Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }

    public class UserBoxConfiguration : IEntityTypeConfiguration<UserBox>
    {
        public void Configure(EntityTypeBuilder<UserBox> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.Box }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Box).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}

using System;
using Izumi.Data.Entities.Resource;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class UserSeed : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity, IAmountEntity
    {
        public Guid Id { get; set; }
        public uint Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public Guid SeedId { get; set; }
        public Seed Seed { get; set; }
    }

    public class UserSeedConfiguration : IEntityTypeConfiguration<UserSeed>
    {
        public void Configure(EntityTypeBuilder<UserSeed> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.SeedId }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Seed)
                .WithMany()
                .HasForeignKey(x => x.SeedId);
        }
    }
}

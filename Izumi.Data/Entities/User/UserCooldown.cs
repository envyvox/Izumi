using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class UserCooldown : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public CooldownType Type { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }

    public class UserCooldownConfiguration : IEntityTypeConfiguration<UserCooldown>
    {
        public void Configure(EntityTypeBuilder<UserCooldown> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.Type }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Expiration).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}

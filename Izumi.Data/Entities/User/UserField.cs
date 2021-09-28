using System;
using Izumi.Data.Entities.Resource;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class UserField : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public uint Number { get; set; }
        public FieldStateType State { get; set; }
        public uint Progress { get; set; }
        public bool InReGrowth { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Guid? SeedId { get; set; }
        public Seed Seed { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }

    public class UserFieldConfiguration : IEntityTypeConfiguration<UserField>
    {
        public void Configure(EntityTypeBuilder<UserField> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.Number }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.Progress).IsRequired();
            builder.Property(x => x.InReGrowth).IsRequired();
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

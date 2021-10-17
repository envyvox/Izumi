using System;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Discord
{
    public class UserWarn : IUniqueIdentifiedEntity, ICreatedEntity
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset Expiration { get; set; }

        public long UserId { get; set; }
        public User.User User { get; set; }

        public long ModeratorId { get; set; }
        public User.User Moderator { get; set; }
    }

    public class UserWarnConfiguration : IEntityTypeConfiguration<UserWarn>
    {
        public void Configure(EntityTypeBuilder<UserWarn> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Reason);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.Expiration).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Moderator)
                .WithMany()
                .HasForeignKey(x => x.ModeratorId);
        }
    }
}

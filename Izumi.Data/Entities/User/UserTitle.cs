using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class UserTitle : IUniqueIdentifiedEntity, ICreatedEntity
    {
        public Guid Id { get; set; }
        public TitleType Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }

    public class UserTitleConfiguration : IEntityTypeConfiguration<UserTitle>
    {
        public void Configure(EntityTypeBuilder<UserTitle> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.Type }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}

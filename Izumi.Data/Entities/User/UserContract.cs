using System;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class UserContract : IUniqueIdentifiedEntity, ICreatedEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset Expiration { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public Guid ContractId { get; set; }
        public Contract Contract { get; set; }
    }

    public class UserContractConfiguration : IEntityTypeConfiguration<UserContract>
    {
        public void Configure(EntityTypeBuilder<UserContract> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.ContractId }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.Expiration).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Contract)
                .WithMany()
                .HasForeignKey(x => x.ContractId);
        }
    }
}

using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Discord
{
    public class ContentVote : IUniqueIdentifiedEntity, ICreatedEntity, IUpdatedEntity
    {
        public Guid Id { get; set; }
        public VoteType Vote { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public long UserId { get; set; }
        public User.User User { get; set; }

        public Guid MessageId { get; set; }
        public ContentMessage Message { get; set; }
    }

    public class ContentVoteConfiguration : IEntityTypeConfiguration<ContentVote>
    {
        public void Configure(EntityTypeBuilder<ContentVote> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.MessageId, x.Vote });

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Vote).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Message)
                .WithMany()
                .HasForeignKey(x => x.MessageId);
        }
    }
}

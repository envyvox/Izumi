using Izumi.Data.Enums.Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Discord
{
    public class Channel
    {
        public long Id { get; set; }
        public DiscordChannelType Type { get; set; }
    }

    public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Type).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Type).IsRequired();
        }
    }
}

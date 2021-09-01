using Izumi.Data.Enums.Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.Discord
{
    public class Role
    {
        public long Id { get; set; }
        public DiscordRoleType Type { get; set; }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Type).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Type).IsRequired();
        }
    }
}

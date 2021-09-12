using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities
{
    public class Achievement : INamedEntity
    {
        public AchievementType Type { get; set; }
        public AchievementCategoryType Category { get; set; }
        public string Name { get; set; }
        public AchievementRewardType RewardType { get; set; }
        public uint RewardNumber { get; set; }
    }

    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.HasKey(x => x.Type);
            builder.HasIndex(x => x.Type).IsUnique();

            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.RewardType).IsRequired();
            builder.Property(x => x.RewardNumber).IsRequired();
        }
    }
}

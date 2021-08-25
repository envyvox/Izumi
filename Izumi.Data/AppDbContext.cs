using System;
using Izumi.Data.Converters;
using Izumi.Data.Entities;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.UseEntityTypeConfiguration<AppDbContext>();
            modelBuilder.UseSnakeCaseNamingConvention();
            modelBuilder.UseValueConverterForType<DateTime>(new DateTimeUtcKindConverter());
        }

        public DbSet<Emote> Emotes { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserBanner> UserBanners { get; set; }

        public DbSet<Banner> Banners { get; set; }
    }
}

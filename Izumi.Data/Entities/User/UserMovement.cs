﻿using System;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Izumi.Data.Entities.User
{
    public class UserMovement : IUniqueIdentifiedEntity, ICreatedEntity
    {
        public Guid Id { get; set; }
        public LocationType Departure { get; set; }
        public LocationType Destination { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }

    public class UserMovementConfiguration : IEntityTypeConfiguration<UserMovement>
    {
        public void Configure(EntityTypeBuilder<UserMovement> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Departure).IsRequired();
            builder.Property(x => x.Destination).IsRequired();
            builder.Property(x => x.Arrival).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}

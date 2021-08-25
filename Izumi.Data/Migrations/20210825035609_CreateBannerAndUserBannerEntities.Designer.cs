﻿// <auto-generated />
using System;
using Izumi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Izumi.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210825035609_CreateBannerAndUserBannerEntities")]
    partial class CreateBannerAndUserBannerEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Izumi.Data.Entities.Banner", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long>("AutoIncrementedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("auto_incremented_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("Price")
                        .HasColumnType("bigint")
                        .HasColumnName("price");

                    b.Property<byte>("Rarity")
                        .HasColumnType("smallint")
                        .HasColumnName("rarity");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_banner");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_banner_name");

                    b.ToTable("banner");
                });

            modelBuilder.Entity("Izumi.Data.Entities.Discord.Emote", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_emotes");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_emotes_name");

                    b.ToTable("emotes");
                });

            modelBuilder.Entity("Izumi.Data.Entities.User.User", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("About")
                        .HasColumnType("text")
                        .HasColumnName("about");

                    b.Property<string>("CommandColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("command_color");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long>("Energy")
                        .HasColumnType("bigint")
                        .HasColumnName("energy");

                    b.Property<byte>("Gender")
                        .HasColumnType("smallint")
                        .HasColumnName("gender");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("boolean")
                        .HasColumnName("is_premium");

                    b.Property<byte>("Location")
                        .HasColumnType("smallint")
                        .HasColumnName("location");

                    b.Property<long>("Points")
                        .HasColumnType("bigint")
                        .HasColumnName("points");

                    b.Property<byte>("Title")
                        .HasColumnType("smallint")
                        .HasColumnName("title");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_users_id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Izumi.Data.Entities.User.UserBanner", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BannerId")
                        .HasColumnType("uuid")
                        .HasColumnName("banner_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_banner");

                    b.HasIndex("BannerId")
                        .HasDatabaseName("ix_user_banner_banner_id");

                    b.HasIndex("UserId", "BannerId", "IsActive")
                        .IsUnique()
                        .HasDatabaseName("ix_user_banner_user_id_banner_id_is_active");

                    b.ToTable("user_banner");
                });

            modelBuilder.Entity("Izumi.Data.Entities.User.UserBanner", b =>
                {
                    b.HasOne("Izumi.Data.Entities.Banner", "Banner")
                        .WithMany()
                        .HasForeignKey("BannerId")
                        .HasConstraintName("fk_user_banner_banner_banner_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Izumi.Data.Entities.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_banner_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Banner");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}

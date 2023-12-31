﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UHC_API.Data;

#nullable disable

namespace UHCAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231103132543_Stats")]
    partial class Stats
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UHC_API.Models.Connection", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int?>("ApplesEaten")
                        .HasColumnType("int");

                    b.Property<int?>("BlocksMined")
                        .HasColumnType("int");

                    b.Property<bool?>("HasWon")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsIronMan")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsTraitor")
                        .HasColumnType("bit");

                    b.Property<string>("KilledBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("KilledFirst")
                        .HasColumnType("bit");

                    b.Property<bool?>("KilledLast")
                        .HasColumnType("bit");

                    b.Property<int?>("Kills")
                        .HasColumnType("int");

                    b.Property<int?>("Position")
                        .HasColumnType("int");

                    b.Property<int?>("TeamColor")
                        .HasColumnType("int");

                    b.Property<int?>("TeamNumber")
                        .HasColumnType("int");

                    b.Property<int?>("TimeKilled")
                        .HasColumnType("int");

                    b.HasKey("PlayerId", "SeasonId");

                    b.HasIndex("SeasonId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("UHC_API.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DiscordId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rank")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("UHC_API.Models.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateHeld")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VictoryType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("UHC_API.Models.Connection", b =>
                {
                    b.HasOne("UHC_API.Models.Player", "Player")
                        .WithMany("Connections")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UHC_API.Models.Season", "Season")
                        .WithMany("Connections")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("UHC_API.Models.Player", b =>
                {
                    b.Navigation("Connections");
                });

            modelBuilder.Entity("UHC_API.Models.Season", b =>
                {
                    b.Navigation("Connections");
                });
#pragma warning restore 612, 618
        }
    }
}

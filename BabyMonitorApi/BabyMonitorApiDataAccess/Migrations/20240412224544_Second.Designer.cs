﻿// <auto-generated />
using System;
using BabyMonitorApiDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    [DbContext(typeof(BabyMonitorContext))]
    [Migration("20240412224544_Second")]
    partial class Second
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.ApiKey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("api_keys", (string)null);
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.Baby", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(300)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("babies", (string)null);
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.MonitoringDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApiKeyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BabyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LivestreamUrl")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApiKeyId")
                        .IsUnique()
                        .HasFilter("[ApiKeyId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("monitoring_devices", (string)null);
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApiKeyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ApiKeyId")
                        .IsUnique()
                        .HasFilter("[ApiKeyId] IS NOT NULL");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.Baby", b =>
                {
                    b.HasOne("BabyMonitorApiDataAccess.Entities.User", "_User")
                        .WithMany("Babies")
                        .HasForeignKey("UserId");

                    b.Navigation("_User");
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.MonitoringDevice", b =>
                {
                    b.HasOne("BabyMonitorApiDataAccess.Entities.ApiKey", "_ApiKey")
                        .WithOne("_MonitoringDevice")
                        .HasForeignKey("BabyMonitorApiDataAccess.Entities.MonitoringDevice", "ApiKeyId");

                    b.HasOne("BabyMonitorApiDataAccess.Entities.User", "_User")
                        .WithMany("MonitoringDevices")
                        .HasForeignKey("UserId");

                    b.Navigation("_ApiKey");

                    b.Navigation("_User");
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.User", b =>
                {
                    b.HasOne("BabyMonitorApiDataAccess.Entities.ApiKey", "ApiKey")
                        .WithOne("_User")
                        .HasForeignKey("BabyMonitorApiDataAccess.Entities.User", "ApiKeyId");

                    b.Navigation("ApiKey");
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.ApiKey", b =>
                {
                    b.Navigation("_MonitoringDevice");

                    b.Navigation("_User");
                });

            modelBuilder.Entity("BabyMonitorApiDataAccess.Entities.User", b =>
                {
                    b.Navigation("Babies");

                    b.Navigation("MonitoringDevices");
                });
#pragma warning restore 612, 618
        }
    }
}
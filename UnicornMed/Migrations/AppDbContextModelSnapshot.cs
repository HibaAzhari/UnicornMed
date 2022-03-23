﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnicornMed.Common.Context;

namespace UnicornMed.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int>("Doctor_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("bit");

                    b.Property<int>("Patient_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DoctorId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.Bot.ConversationReferenceEntity", b =>
                {
                    b.Property<string>("UPN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AadObjectId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActivityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BotId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChannelId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConversationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartitionKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RowKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UPN");

                    b.ToTable("ConversationReferenceEntities");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AltEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Admin", b =>
                {
                    b.HasOne("UnicornMed.Common.Models.Database.API.Admin", null)
                        .WithMany("Admins")
                        .HasForeignKey("AdminId");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Booking", b =>
                {
                    b.HasOne("UnicornMed.Common.Models.Database.API.Booking", null)
                        .WithMany("Bookings")
                        .HasForeignKey("BookingId");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Doctor", b =>
                {
                    b.HasOne("UnicornMed.Common.Models.Database.API.Doctor", null)
                        .WithMany("Doctors")
                        .HasForeignKey("DoctorId");
                });

            modelBuilder.Entity("UnicornMed.Common.Models.Database.API.Patient", b =>
                {
                    b.HasOne("UnicornMed.Common.Models.Database.API.Patient", null)
                        .WithMany("Patients")
                        .HasForeignKey("PatientId");
                });
#pragma warning restore 612, 618
        }
    }
}

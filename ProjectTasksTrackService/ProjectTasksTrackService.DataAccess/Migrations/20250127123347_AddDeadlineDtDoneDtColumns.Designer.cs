﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectTasksTrackService.DataAccess;

#nullable disable

namespace ProjectTasksTrackService.DataAccess.Migrations
{
    [DbContext(typeof(ProjectTasksTrackServiceDbContext))]
    [Migration("20250127123347_AddDeadlineDtDoneDtColumns")]
    partial class AddDeadlineDtDoneDtColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectTasksTrackService.DataAccess.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("CreatedDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeadLineDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DoneDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<DateTime?>("LastUpdateDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Url")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Code");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectTasksTrackService.DataAccess.Entities.ProjectSubDivision", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("CreatedDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeadLineDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DoneDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<DateTime?>("LastUpdateDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<string>("Url1")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("Url2")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectSubDivisions");
                });

            modelBuilder.Entity("ProjectTasksTrackService.DataAccess.Entities.ProjectTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeadLineDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DoneDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<DateTime?>("LastUpdateDt")
                        .HasMaxLength(25)
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProjectSubDivisionId")
                        .HasColumnType("integer");

                    b.Property<string>("Url1")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("Url2")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ProjectSubDivisionId");

                    b.ToTable("ProjectTasks");
                });

            modelBuilder.Entity("ProjectTasksTrackService.DataAccess.Entities.ProjectSubDivision", b =>
                {
                    b.HasOne("ProjectTasksTrackService.DataAccess.Entities.Project", "Project")
                        .WithMany("ProjectSubDivisions")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectTasksTrackService.DataAccess.Entities.ProjectTask", b =>
                {
                    b.HasOne("ProjectTasksTrackService.DataAccess.Entities.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ProjectTasksTrackService.DataAccess.Entities.ProjectSubDivision", "ProjectSubDivision")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectSubDivisionId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Project");

                    b.Navigation("ProjectSubDivision");
                });

            modelBuilder.Entity("ProjectTasksTrackService.DataAccess.Entities.Project", b =>
                {
                    b.Navigation("ProjectSubDivisions");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ProjectTasksTrackService.DataAccess.Entities.ProjectSubDivision", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}

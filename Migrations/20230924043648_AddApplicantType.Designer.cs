﻿// <auto-generated />
using System;
using ERecruitmentBE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ERecruitmentBE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230924043648_AddApplicantType")]
    partial class AddApplicantType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ERecruitmentBE.Models.ApplicantSpecification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("ApplicantSpecifications");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.ApplicantSpecificationItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicantSpecificationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantSpecificationId");

                    b.ToTable("ApplicantSpecificationItems");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.Candidate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AIScreeningStatus")
                        .HasColumnType("int");

                    b.Property<int>("ApplicantSpecApprove")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdJobVacancy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoHandphone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.CandidateSpecification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicantItemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicantItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CandidateId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("CandidateSpecifications");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.JobVacancy", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("JobVacancys");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.Requirement", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicantSpecificationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicantSpecificationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("JobVacancyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("JobVacancyId");

                    b.ToTable("Requirements");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.ApplicantSpecificationItem", b =>
                {
                    b.HasOne("ERecruitmentBE.Models.ApplicantSpecification", null)
                        .WithMany("ListApplicantSpecificationsItem")
                        .HasForeignKey("ApplicantSpecificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ERecruitmentBE.Models.Requirement", b =>
                {
                    b.HasOne("ERecruitmentBE.Models.JobVacancy", null)
                        .WithMany("ListRequirement")
                        .HasForeignKey("JobVacancyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ERecruitmentBE.Models.ApplicantSpecification", b =>
                {
                    b.Navigation("ListApplicantSpecificationsItem");
                });

            modelBuilder.Entity("ERecruitmentBE.Models.JobVacancy", b =>
                {
                    b.Navigation("ListRequirement");
                });
#pragma warning restore 612, 618
        }
    }
}

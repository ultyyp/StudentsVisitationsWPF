﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentsVisitationsWPF.Entities;

#nullable disable

namespace StudentsVisitationsWPF.Migrations
{
    [DbContext(typeof(DBMethods.AppDbContext))]
    [Migration("20230227130303_MigrationName")]
    partial class MigrationName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.14");

            modelBuilder.Entity("StudentsVisitationsWPF.DBMethods+Student", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<string>("EMAIL")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FIO")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentsVisitationsWPF.DBMethods+Visitation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("DATE")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("STUDENTID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Visitations");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Unidash.TimeTable;

namespace Unidash.TimeTable.Application.Migrations
{
    [DbContext(typeof(TimeTableDbContext))]
    [Migration("20200609120437_AddIsHiddenProperty")]
    partial class AddIsHiddenProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Unidash.TimeTable.Models.CalendarEntry", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndsAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartsAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CalendarEntries");
                });
#pragma warning restore 612, 618
        }
    }
}

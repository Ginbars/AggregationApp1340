﻿// <auto-generated />
using AggregationApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AggregationApp.Migrations
{
    [DbContext(typeof(AggregatedDataContext))]
    [Migration("20230813161803_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("AggregationApp.AggregatedData", b =>
                {
                    b.Property<string>("Region")
                        .HasColumnType("TEXT");

                    b.Property<float>("PMinusSum")
                        .HasColumnType("REAL");

                    b.Property<float>("PPlusSum")
                        .HasColumnType("REAL");

                    b.HasKey("Region");

                    b.ToTable("AData");
                });
#pragma warning restore 612, 618
        }
    }
}
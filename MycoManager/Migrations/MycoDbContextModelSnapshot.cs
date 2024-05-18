﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MycoManager.Data;

#nullable disable

namespace MycoManager.Migrations
{
    [DbContext(typeof(MycoDbContext))]
    partial class MycoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MycoManager.Models.EdgeSensor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SetCO2PPM")
                        .HasColumnType("int");

                    b.Property<double>("SetHumidity")
                        .HasColumnType("float");

                    b.Property<double>("SetTemperature")
                        .HasColumnType("float");

                    b.Property<string>("Uri")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("EdgeSensors");
                });

            modelBuilder.Entity("MycoManager.Models.MycoStrain", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("S3ObjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Species")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("MycoStrains");
                });
#pragma warning restore 612, 618
        }
    }
}

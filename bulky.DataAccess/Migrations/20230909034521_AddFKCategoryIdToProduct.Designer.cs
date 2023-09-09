﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using bulky.DataAccess.Data;

#nullable disable

namespace bulky.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230909034521_AddFKCategoryIdToProduct")]
    partial class AddFKCategoryIdToProduct
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("bulkyBook.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Action"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "SciFi"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "History"
                        });
                });

            modelBuilder.Entity("bulkyBook.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("ListPrice")
                        .HasColumnType("double precision");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<double>("Price100")
                        .HasColumnType("double precision");

                    b.Property<double>("Price50")
                        .HasColumnType("double precision");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "Kathy Sierra",
                            CategoryId = 1,
                            Description = "Head First Java is a complete learning experience in Java and object-oriented programming.",
                            ISBN = "1491910771",
                            ListPrice = 39.0,
                            Price = 37.0,
                            Price100 = 30.0,
                            Price50 = 35.0,
                            Title = "Head First Java"
                        },
                        new
                        {
                            Id = 2,
                            Author = "Jon Skeet",
                            CategoryId = 2,
                            Description = "C# in Depth, Fourth Edition is an authoritative and engaging guide that reveals the full potential of the language, including the new features of C# 6 and 7.",
                            ISBN = "1617294535",
                            ListPrice = 45.0,
                            Price = 42.0,
                            Price100 = 38.0,
                            Price50 = 40.0,
                            Title = "C# in Depth"
                        },
                        new
                        {
                            Id = 3,
                            Author = "Luciano Ramalho",
                            CategoryId = 3,
                            Description = "Author Luciano Ramalho takes you through Python's core language features and libraries, and shows you how to make your code shorter, faster, and more readable.",
                            ISBN = "1491946008",
                            ListPrice = 40.0,
                            Price = 38.0,
                            Price100 = 35.0,
                            Price50 = 37.0,
                            Title = "Fluent Python"
                        });
                });

            modelBuilder.Entity("bulkyBook.Models.Product", b =>
                {
                    b.HasOne("bulkyBook.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}

using bulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace bulky.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }

  public DbSet<Category> Categories { get; set; }
  public DbSet<Product> Products { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Category>().HasData(
      new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
      new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
      new Category { Id = 3, Name = "History", DisplayOrder = 3 }

    );

    modelBuilder.Entity<Product>().HasData(
      new Product
      {
        Id = 1,
        Title = "Head First Java",
        Author = "Kathy Sierra",
        Description = "Head First Java is a complete learning experience in Java and object-oriented programming.",
        ISBN = "1491910771",
        ListPrice = 39.00,
        Price = 37.00,
        Price50 = 35.00,
        Price100 = 30.00
      },
      new Product
      {
        Id = 2,
        Title = "C# in Depth",
        Author = "Jon Skeet",
        Description = "C# in Depth, Fourth Edition is an authoritative and engaging guide that reveals the full potential of the language, including the new features of C# 6 and 7.",
        ISBN = "1617294535",
        ListPrice = 45.00,
        Price = 42.00,
        Price50 = 40.00,
        Price100 = 38.00
      },
      new Product
      {
        Id = 3,
        Title = "Fluent Python",
        Author = "Luciano Ramalho",
        Description = "Author Luciano Ramalho takes you through Python's core language features and libraries, and shows you how to make your code shorter, faster, and more readable.",
        ISBN = "1491946008",
        ListPrice = 40.00,
        Price = 38.00,
        Price50 = 37.00,
        Price100 = 35.00
      }
    );
  }
}

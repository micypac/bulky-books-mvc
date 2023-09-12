using bulkyBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bulky.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }

  public DbSet<Category> Categories { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Company> Companies { get; set; }
  public DbSet<ApplicationUser> ApplicationUsers { get; set; }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder); // this is required by IdentityDbContext or it will throw an error

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
        Price100 = 30.00,
        CategoryId = 1,
        ImageUrl = ""
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
        Price100 = 38.00,
        CategoryId = 2,
        ImageUrl = ""
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
        Price100 = 35.00,
        CategoryId = 3,
        ImageUrl = ""
      }
    );

    modelBuilder.Entity<Company>().HasData(
      new Company
      {
        Id = 1,
        Name = "Tech Solutions",
        PhoneNumber = "6748779655",
        StreetAddress = "123 Tech St",
        City = "Tech City",
        State = "IL",
        PostalCode = "12121"
      },
      new Company
      {
        Id = 2,
        Name = "Vivid Books",
        PhoneNumber = "7045564122",
        StreetAddress = "99 Vid St.",
        City = "Vid City",
        State = "CA",
        PostalCode = "92501"
      },
      new Company
      {
        Id = 3,
        Name = "Readers Club",
        PhoneNumber = "9410095656",
        StreetAddress = "466 Drew Blvd",
        City = "Hoboken",
        State = "NJ",
        PostalCode = "09678"
      }
    );
  }
}

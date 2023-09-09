using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Kathy Sierra", "Head First Java is a complete learning experience in Java and object-oriented programming.", "1491910771", 39.0, 37.0, 30.0, 35.0, "Head First Java" },
                    { 2, "Jon Skeet", "C# in Depth, Fourth Edition is an authoritative and engaging guide that reveals the full potential of the language, including the new features of C# 6 and 7.", "1617294535", 45.0, 42.0, 38.0, 40.0, "C# in Depth" },
                    { 3, "Luciano Ramalho", "Author Luciano Ramalho takes you through Python's core language features and libraries, and shows you how to make your code shorter, faster, and more readable.", "1491946008", 40.0, 38.0, 35.0, 37.0, "Fluent Python" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

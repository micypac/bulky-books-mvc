using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetailColumnToOrderHeaderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OderHeaderId",
                table: "OrderDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OderHeaderId",
                table: "OrderDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

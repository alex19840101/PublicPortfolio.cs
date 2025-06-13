using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Product_Add_PriceId_PricePerUnit_integer_ids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerUnit",
                table: "Products",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "Products");
        }
    }
}

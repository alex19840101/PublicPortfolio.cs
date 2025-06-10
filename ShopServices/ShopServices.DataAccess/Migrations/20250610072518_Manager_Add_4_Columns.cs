using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Manager_Add_4_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientGroups",
                table: "Managers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodsCategories",
                table: "Managers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "Managers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkStatus",
                table: "Managers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientGroups",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "GoodsCategories",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "WorkStatus",
                table: "Managers");

        }
    }
}

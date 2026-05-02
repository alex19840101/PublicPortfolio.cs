using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Availability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityTownCode",
                table: "Availabilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Availabilities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSupplyTime",
                table: "Availabilities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Availabilities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextSupplyTime",
                table: "Availabilities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceName",
                table: "Availabilities",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Availabilities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_ManagerId",
                table: "Availabilities",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Managers_ManagerId",
                table: "Availabilities",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Managers_ManagerId",
                table: "Availabilities");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_ManagerId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "CityTownCode",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "LastSupplyTime",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "NextSupplyTime",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "PlaceName",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Availabilities");
        }
    }
}

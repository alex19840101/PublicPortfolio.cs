using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Shops_Warehouses_Deliveries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Warehouses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDt",
                table: "Warehouses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionCode",
                table: "Warehouses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Warehouses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkSchedule",
                table: "Warehouses",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Shops",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDt",
                table: "Shops",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionCode",
                table: "Shops",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Shops",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkSchedule",
                table: "Shops",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");


            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Deliveries",
                type: "character varying(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FromShopId",
                table: "Deliveries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromWarehouseId",
                table: "Deliveries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionCode",
                table: "Deliveries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Deliveries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToShopId",
                table: "Deliveries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToWarehouseId",
                table: "Deliveries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransferId",
                table: "Deliveries",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopId",
                table: "Orders",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ShopId",
                table: "Employees",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WarehouseId",
                table: "Employees",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Shops_ShopId",
                table: "Employees",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Warehouses_WarehouseId",
                table: "Employees",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Shops_ShopId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Warehouses_WarehouseId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ShopId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_WarehouseId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedDt",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WorkSchedule",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "CreatedDt",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "WorkSchedule",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "FromShopId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "FromWarehouseId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ToShopId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ToWarehouseId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "TransferId",
                table: "Deliveries");
        }
    }
}

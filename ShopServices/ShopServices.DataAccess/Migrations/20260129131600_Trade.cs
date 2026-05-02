using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Trade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Trades",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Trades",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "Trades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Trades",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourierId",
                table: "Trades",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Trades",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Trades",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExtraInfo",
                table: "Trades",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Trades",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Trades",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentInfo",
                table: "Trades",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Positions",
                table: "Trades",
                type: "character varying(16384)",
                maxLength: 16384,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "RefundAmount",
                table: "Trades",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundDateTime",
                table: "Trades",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundInfo",
                table: "Trades",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Trades",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trades_BuyerId",
                table: "Trades",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_OrderId",
                table: "Trades",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_ShopId",
                table: "Trades",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Buyers_BuyerId",
                table: "Trades",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Orders_OrderId",
                table: "Trades",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Shops_ShopId",
                table: "Trades",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Buyers_BuyerId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Orders_OrderId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Shops_ShopId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_BuyerId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_OrderId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_ShopId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ExtraInfo",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "PaymentInfo",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Positions",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "RefundAmount",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "RefundDateTime",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "RefundInfo",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Trades");
        }
    }
}

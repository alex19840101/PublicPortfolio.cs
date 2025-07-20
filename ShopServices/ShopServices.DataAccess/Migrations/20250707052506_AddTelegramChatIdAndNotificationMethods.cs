using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramChatIdAndNotificationMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<byte>>(
                name: "NotificationMethods",
                table: "Employees",
                type: "smallint[]",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TelegramChatId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<List<byte>>(
                name: "NotificationMethods",
                table: "Buyers",
                type: "smallint[]",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TelegramChatId",
                table: "Buyers",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationMethods",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TelegramChatId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NotificationMethods",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "TelegramChatId",
                table: "Buyers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NotificationsDev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "PhoneNotifications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChangedEntityId",
                table: "PhoneNotifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "PhoneNotifications",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUnsuccessfulAttempt",
                table: "PhoneNotifications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModelEntityType",
                table: "PhoneNotifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotificationMethod",
                table: "PhoneNotifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnsuccessfulAttempts",
                table: "PhoneNotifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "EmailNotifications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChangedEntityId",
                table: "EmailNotifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "EmailNotifications",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUnsuccessfulAttempt",
                table: "EmailNotifications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModelEntityType",
                table: "EmailNotifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "EmailNotifications",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UnsuccessfulAttempts",
                table: "EmailNotifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "PhoneNotifications");

            migrationBuilder.DropColumn(
                name: "ChangedEntityId",
                table: "PhoneNotifications");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "PhoneNotifications");

            migrationBuilder.DropColumn(
                name: "LastUnsuccessfulAttempt",
                table: "PhoneNotifications");

            migrationBuilder.DropColumn(
                name: "ModelEntityType",
                table: "PhoneNotifications");

            migrationBuilder.DropColumn(
                name: "NotificationMethod",
                table: "PhoneNotifications");

            migrationBuilder.DropColumn(
                name: "UnsuccessfulAttempts",
                table: "PhoneNotifications");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "ChangedEntityId",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "LastUnsuccessfulAttempt",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "ModelEntityType",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "UnsuccessfulAttempts",
                table: "EmailNotifications");
        }
    }
}

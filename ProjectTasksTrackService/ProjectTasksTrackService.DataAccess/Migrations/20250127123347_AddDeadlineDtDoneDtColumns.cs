using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTasksTrackService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDeadlineDtDoneDtColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DoneDt",
                table: "ProjectTasks",
                type: "timestamp with time zone",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DoneDt",
                table: "ProjectSubDivisions",
                type: "timestamp with time zone",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeadLineDt",
                table: "Projects",
                type: "timestamp with time zone",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DoneDt",
                table: "Projects",
                type: "timestamp with time zone",
                maxLength: 25,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoneDt",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "DoneDt",
                table: "ProjectSubDivisions");

            migrationBuilder.DropColumn(
                name: "DeadLineDt",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DoneDt",
                table: "Projects");
        }
    }
}

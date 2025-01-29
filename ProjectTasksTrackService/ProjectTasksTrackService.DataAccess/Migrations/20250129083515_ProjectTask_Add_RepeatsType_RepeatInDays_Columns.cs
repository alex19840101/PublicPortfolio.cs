using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTasksTrackService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTask_Add_RepeatsType_RepeatInDays_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepeatInDays",
                table: "ProjectTasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RepeatsType",
                table: "ProjectTasks",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepeatInDays",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "RepeatsType",
                table: "ProjectTasks");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTasksTrackService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectSubDivisionColumnCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ProjectSubDivisions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "ProjectSubDivisions");
        }
    }
}

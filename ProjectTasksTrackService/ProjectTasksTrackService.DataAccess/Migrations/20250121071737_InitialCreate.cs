using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectTasksTrackService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true),
                    LastUpdateDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.UniqueConstraint("AK_Projects_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSubDivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", maxLength: 255, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url1 = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Url2 = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true),
                    LastUpdateDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true),
                    DeadLineDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSubDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSubDivisions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", maxLength: 255, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    ProjectSubDivisionId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url1 = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Url2 = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true),
                    LastUpdateDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true),
                    DeadLineDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_ProjectSubDivisions_ProjectSubDivisionId",
                        column: x => x.ProjectSubDivisionId,
                        principalTable: "ProjectSubDivisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSubDivisions_ProjectId",
                table: "ProjectSubDivisions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectSubDivisionId",
                table: "ProjectTasks",
                column: "ProjectSubDivisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "ProjectSubDivisions");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}

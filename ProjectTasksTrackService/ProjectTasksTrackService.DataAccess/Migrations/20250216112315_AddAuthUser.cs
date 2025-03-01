using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectTasksTrackService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Nick = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Role = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    GranterId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: false),
                    LastUpdateDt = table.Column<DateTime>(type: "timestamp with time zone", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.Id);
                    table.UniqueConstraint("AK_AuthUsers_Login", x => x.Login);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthUsers");
        }
    }
}

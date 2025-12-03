using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcheryAcademy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatusColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "users",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "users");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "users",
                type: "boolean",
                nullable: true,
                defaultValue: true);
        }
    }
}

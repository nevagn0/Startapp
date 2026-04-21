using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameRenameRolesAndRanksv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Games",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "Rang",
                table: "Games",
                newName: "Ranks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "Games",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Ranks",
                table: "Games",
                newName: "Rang");
        }
    }
}

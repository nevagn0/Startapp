using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeIsRatingAvailableBeeingProprety : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRatingAvailable",
                table: "Lobbies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRatingAvailable",
                table: "Lobbies",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

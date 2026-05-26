using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PremiumFlagForUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPremium",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_HasPremium",
                table: "Profiles",
                column: "HasPremium");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_HasPremium",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "HasPremium",
                table: "Profiles");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProfileAvatarUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Profiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Profiles",
                type: "text",
                nullable: true);
        }
    }
}

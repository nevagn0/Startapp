using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdToProfileIdv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Profiles_UserId",
                table: "UserGames");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserGames",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGames_UserId_GameId",
                table: "UserGames",
                newName: "IX_UserGames_ProfileId_GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Profiles_ProfileId",
                table: "UserGames",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Profiles_ProfileId",
                table: "UserGames");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "UserGames",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGames_ProfileId_GameId",
                table: "UserGames",
                newName: "IX_UserGames_UserId_GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Profiles_UserId",
                table: "UserGames",
                column: "UserId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

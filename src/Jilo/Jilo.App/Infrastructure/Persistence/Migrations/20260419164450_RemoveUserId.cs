using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Profiles_ProfileId",
                table: "UserGames");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_ProfileId",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "UserGames");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Profiles_UserId",
                table: "UserGames",
                column: "UserId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Profiles_UserId",
                table: "UserGames");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "UserGames",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_ProfileId",
                table: "UserGames",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Profiles_ProfileId",
                table: "UserGames",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

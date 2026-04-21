using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LobbyBuisnessLogicFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LeftAtUtc",
                table: "LobbyMembers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRatingAvailable",
                table: "Lobbies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAtUtc",
                table: "Lobbies",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftAtUtc",
                table: "LobbyMembers");

            migrationBuilder.DropColumn(
                name: "IsRatingAvailable",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "StartedAtUtc",
                table: "Lobbies");
        }
    }
}

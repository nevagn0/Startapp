using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeForIsRatingAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedAtUtc",
                table: "Lobbies",
                newName: "RatingAvailableUntilUtc");

            migrationBuilder.AddColumn<DateTime>(
                name: "RatingAvailableSinceUtc",
                table: "Lobbies",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingAvailableSinceUtc",
                table: "Lobbies");

            migrationBuilder.RenameColumn(
                name: "RatingAvailableUntilUtc",
                table: "Lobbies",
                newName: "StartedAtUtc");
        }
    }
}

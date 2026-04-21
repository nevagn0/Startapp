using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLobbyMemberLeftAtUtcField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftAtUtc",
                table: "LobbyMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LeftAtUtc",
                table: "LobbyMembers",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}

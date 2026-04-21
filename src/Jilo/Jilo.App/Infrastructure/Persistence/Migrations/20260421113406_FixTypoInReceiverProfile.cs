using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jilo.App.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoInReceiverProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Profiles_RecieverProfileId",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "RecieverProfileId",
                table: "Invitations",
                newName: "ReceiverProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_RecieverProfileId_Status",
                table: "Invitations",
                newName: "IX_Invitations_ReceiverProfileId_Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Profiles_ReceiverProfileId",
                table: "Invitations",
                column: "ReceiverProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Profiles_ReceiverProfileId",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "ReceiverProfileId",
                table: "Invitations",
                newName: "RecieverProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_ReceiverProfileId_Status",
                table: "Invitations",
                newName: "IX_Invitations_RecieverProfileId_Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Profiles_RecieverProfileId",
                table: "Invitations",
                column: "RecieverProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class EditSomeConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientOffers_Clients_ClientId",
                table: "ClientOffers");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientOffers_Clients_ClientId",
                table: "ClientOffers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientOffers_Clients_ClientId",
                table: "ClientOffers");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientOffers_Clients_ClientId",
                table: "ClientOffers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

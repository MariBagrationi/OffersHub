using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RenameOfferClientDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProducts_Clients_ClientId",
                table: "ClientProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientProducts_Offers_OfferId",
                table: "ClientProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientProducts",
                table: "ClientProducts");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "ClientProducts",
                newName: "ClientOffers");

            migrationBuilder.RenameIndex(
                name: "IX_ClientProducts_OfferId",
                table: "ClientOffers",
                newName: "IX_ClientOffers_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientOffers",
                table: "ClientOffers",
                columns: new[] { "ClientId", "OfferId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientOffers_Clients_ClientId",
                table: "ClientOffers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientOffers_Offers_OfferId",
                table: "ClientOffers",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientOffers_Clients_ClientId",
                table: "ClientOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientOffers_Offers_OfferId",
                table: "ClientOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientOffers",
                table: "ClientOffers");

            migrationBuilder.RenameTable(
                name: "ClientOffers",
                newName: "ClientProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ClientOffers_OfferId",
                table: "ClientProducts",
                newName: "IX_ClientProducts_OfferId");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientProducts",
                table: "ClientProducts",
                columns: new[] { "ClientId", "OfferId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProducts_Clients_ClientId",
                table: "ClientProducts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProducts_Offers_OfferId",
                table: "ClientProducts",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

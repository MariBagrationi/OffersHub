using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClientOfferKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientOffers",
                table: "ClientOffers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ClientOffers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientOffers",
                table: "ClientOffers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOffers_ClientId",
                table: "ClientOffers",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientOffers",
                table: "ClientOffers");

            migrationBuilder.DropIndex(
                name: "IX_ClientOffers_ClientId",
                table: "ClientOffers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ClientOffers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientOffers",
                table: "ClientOffers",
                columns: new[] { "ClientId", "OfferId" });
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clients");
        }
    }
}

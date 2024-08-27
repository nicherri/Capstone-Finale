using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelEasy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Areas_AreaId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shelves_ShelfId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shelvings_ShelvingId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Shelvings_ShelvingId",
                table: "Shelves");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelvings_Areas_AreaId",
                table: "Shelvings");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Areas_AreaId",
                table: "Products",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shelves_ShelfId",
                table: "Products",
                column: "ShelfId",
                principalTable: "Shelves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shelvings_ShelvingId",
                table: "Products",
                column: "ShelvingId",
                principalTable: "Shelvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Shelvings_ShelvingId",
                table: "Shelves",
                column: "ShelvingId",
                principalTable: "Shelvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelvings_Areas_AreaId",
                table: "Shelvings",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Areas_AreaId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shelves_ShelfId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shelvings_ShelvingId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Shelvings_ShelvingId",
                table: "Shelves");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelvings_Areas_AreaId",
                table: "Shelvings");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Areas_AreaId",
                table: "Products",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shelves_ShelfId",
                table: "Products",
                column: "ShelfId",
                principalTable: "Shelves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shelvings_ShelvingId",
                table: "Products",
                column: "ShelvingId",
                principalTable: "Shelvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Shelvings_ShelvingId",
                table: "Shelves",
                column: "ShelvingId",
                principalTable: "Shelvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelvings_Areas_AreaId",
                table: "Shelvings",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

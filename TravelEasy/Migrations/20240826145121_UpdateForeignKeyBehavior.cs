using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelEasy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignKeyBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Shelvings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShelvingId",
                table: "Shelves",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shelvings_AreaId",
                table: "Shelvings",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_ShelvingId",
                table: "Shelves",
                column: "ShelvingId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Shelvings_ShelvingId",
                table: "Shelves");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelvings_Areas_AreaId",
                table: "Shelvings");

            migrationBuilder.DropIndex(
                name: "IX_Shelvings_AreaId",
                table: "Shelvings");

            migrationBuilder.DropIndex(
                name: "IX_Shelves_ShelvingId",
                table: "Shelves");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Shelvings");

            migrationBuilder.DropColumn(
                name: "ShelvingId",
                table: "Shelves");
        }
    }
}
